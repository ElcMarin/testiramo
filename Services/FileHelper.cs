namespace maturitetna.Services;

public class FileHelper
{
    static public string GetProfilePicture( int id, char rights)
    {
        //kak nardim poizvedbo tule notr da vidim v kateri tabeli je pac po pravicah
        if (System.IO.File.Exists("wwwroot/Storage/ProfilePics/" + id + "_" + rights + ".png"))
        {
            return "/Storage/ProfilePics/" + id + "_" + rights + ".png?" + DateTime.UtcNow.Ticks;
        }
        else
        {
            return "/Storage/ProfilePics/default.png";
        }
    }

    static public string GetHaircutPicture(int id)
    {
        if (System.IO.File.Exists("/Storage/HaircutPics/" + id + ".png"))
        {
            return "/Storage/HaircutPics/"  + id + ".png?" + DateTime.UtcNow.Ticks;
        }
        else
        {
            return "/Storage/HaircutPics/default.png";
        }
    }

    static public string GetHairdresserPicture(int id)
    {
        if (System.IO.File.Exists("/Storage/HairdresserPics/" + id + ".png"))
        {
            return "/Storage/HairdresserPics/"  + id + ".png?" + DateTime.UtcNow.Ticks;
        }
        else
        {
            return "/Storage/HairdresserPics/default.png";
        }
    }
}