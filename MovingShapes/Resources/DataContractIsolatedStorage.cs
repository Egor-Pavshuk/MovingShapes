using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace MovingShapes.Resources
{
    internal class DataContractIsolatedStorage
    {
        public static void WriteToFile<T>(T model) where T : class
        {
            using (var storage = IsolatedStorageFile.GetMachineStoreForAssembly())
            {
                using (var stream = new IsolatedStorageFileStream("RandomPoint.data", System.IO.FileMode.OpenOrCreate, storage))
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(stream, model);
                }
            }
        }

        public static T? ReadFromFile<T>() where T : class
        {
            using (var storage = IsolatedStorageFile.GetMachineStoreForAssembly())
            {
                using (var stream = new IsolatedStorageFileStream("RandomPoint.data", System.IO.FileMode.OpenOrCreate, storage))
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    var readedObject = (T?)serializer.ReadObject(stream);
                    return readedObject;
                }
            }
        }
    }
}
