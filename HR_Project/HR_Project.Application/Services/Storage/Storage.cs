using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_Project.Application.Operations;

namespace HR_Project.Application.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);
        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod, bool first = true)
        {
            string newFileName = await Task.Run(async () =>
            {
                string extension = Path.GetExtension(fileName);

                string newFileName = string.Empty;

                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);

                    newFileName = NameOperation.CharacterRegulatory(oldName) + extension;
                }
                else
                {
                    newFileName = fileName;

                    int index1 = newFileName.LastIndexOf('-');
                    int index2 = newFileName.IndexOf(".");

                    string number = newFileName.Substring(index1 + 1, index2 - index1 - 1);



                    if (index1 == -1 || !int.TryParse(number, out int numberInt))
                    {
                        newFileName = Path.GetFileNameWithoutExtension(fileName) + "-2" + extension;
                    }
                    else
                    {
                        numberInt++;

                        newFileName = newFileName.Remove(index1 + 1, index2 - index1 - 1).Insert(index1 + 1, numberInt.ToString());
                    }

                }

                //if (File.Exists($"{pathOrContainerName}\\{newFileName}"))
                if (hasFileMethod(pathOrContainerName, newFileName))
                {
                    return await FileRenameAsync(pathOrContainerName, newFileName, hasFileMethod, false);
                }
                else
                    return newFileName;
            });
            return newFileName;
        }
    }
}
