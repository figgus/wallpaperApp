
using wallpaperApp;

try
{
    var wallpaper = new Wallpaper();

    wallpaper.initApp();

    string nombreRandom = wallpaper.GetNombreDeImagenRandom();
    wallpaper.actualizarUltimaImagen(nombreRandom);

    wallpaper.CambiarImagen(nombreRandom);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
