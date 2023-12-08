using System.Runtime.InteropServices;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace wallpaperApp;

public class Wallpaper
{

    private string obtenerRutaJson()
    {
        string rutaLocal = GetProjectPath();
        rutaLocal += "\\data.json";
        return rutaLocal;
    }
    public void initApp()
    {
        string rutaLocal = obtenerRutaJson();
        if (!File.Exists(rutaLocal))
        {
            File.Create(rutaLocal);
            string texto = JsonConvert.SerializeObject(new JsonModel());

        }



    }

    public void actualizarUltimaImagen(string nombreImagen)
    {
        string rutaLocal = obtenerRutaJson();
        JsonModel model = new JsonModel
        {
            UltimaImagenUsada = nombreImagen
        };
        string data = JsonConvert.SerializeObject(model);
        File.WriteAllText(rutaLocal, data);
    }

    private string GetProjectPath()
    {
        string assemblyPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        string projectPath = Path.GetDirectoryName(assemblyPath);
        if (string.IsNullOrEmpty(projectPath))
        {
            throw new Exception("No se encontro ruta de la aplicacion");
        }
        return projectPath;
    }


    public string GetNombreDeImagenRandom()
    {

        Console.WriteLine("Cambiando wallpaper!");
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        folderPath += "\\wallpapers";

        Console.WriteLine("buscando imagenes en " + folderPath);

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("No se encontro la ruta");
            throw new FileNotFoundException("No se encontro la ruta");
        }
        // Obtener todas las imágenes de la carpeta
        string[] imageFiles = Directory.GetFiles(folderPath);

        if (imageFiles.Length == 0)
        {
            Console.WriteLine("No se encontraron imágenes en la carpeta especificada.");
            throw new Exception();
        }

        // Obtener una imagen aleatoria
        Random random = new Random();


        string randomImage = imageFiles[random.Next(imageFiles.Length)];
        string ultimaImagen = ObtenerUltimaImagenUsada(folderPath);
        while (randomImage == ultimaImagen)
        {
            Console.WriteLine(randomImage + " se repitio, buscando otra imagen");
            randomImage = imageFiles[random.Next(imageFiles.Length)];
        }
        Console.WriteLine(" la imagen elegida es " + randomImage);

        return randomImage;
    }

    public void CambiarImagen(string rutaImagen)
    {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true))
        {
            if (key != null)
            {
                key.SetValue("WallpaperStyle", "2");  // Estirar la imagen para ajustar al escritorio
                key.SetValue("TileWallpaper", "0");   // No repetir la imagen en mosaico

                // Establecer la ruta de la imagen como fondo de escritorio
                key.SetValue("Wallpaper", rutaImagen);
            }
        }

        // Actualizar el fondo de escritorio
        SystemParametersInfo(0x14, 0, null, 0x01 | 0x02);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }


    private string ObtenerUltimaImagenUsada(string rutaUltimaImagen)
    {

        string res = "";

        string rutaLocal = GetProjectPath();
        rutaLocal += "\\data.json";
        res = File.ReadAllText(rutaLocal );
        var obj = JsonConvert.DeserializeObject<JsonModel>(res);
        return obj.UltimaImagenUsada;
    }



}
