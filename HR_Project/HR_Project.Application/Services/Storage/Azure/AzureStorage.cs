using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using HR_Project.Domain.Services.StorageService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HR_Project.Application.Services.Storage.Azure
{
    public class AzureStorage : Storage, IAzureStorage
    {
		readonly BlobServiceClient _blobServiceClient;
		BlobContainerClient _blobContainerClient;
		public AzureStorage(IConfiguration configuration)
		{
			_blobServiceClient = new(configuration["Storage:Azure"]);
		}
		public async Task DeleteAsync(string containerName, string fileName)
		{
			_blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
			await blobClient.DeleteAsync();
		}

		public List<string> GetFiles(string containerName)
		{
			_blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			return _blobContainerClient.GetBlobs().Select(b => b.Name).ToList();
		}

		public bool HasFile(string containerName, string fileName)
		{
			_blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			return _blobContainerClient.GetBlobs().Any(b => b.Name == fileName);
		}

		public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string containerName, IFormFile file)
		{

			_blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			await _blobContainerClient.CreateIfNotExistsAsync();
			await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

			List<(string fileName, string path)> datas = new List<(string fileName, string path)>();

			

				var fileNewName = await FileRenameAsync(containerName, file.FileName, HasFile);

				BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
				await blobClient.UploadAsync(file.OpenReadStream());
				datas.Add((fileNewName, $"{containerName}/{fileNewName}"));
			

			return datas;
		}
	}
}
