namespace maturitetna.Services;

public class FileHelper
{
    static public string GetProfilePicture( int id, char rights)
    {
        //kak nardim poizvedbo tule notr da vidim v kateri tabeli je pac po pravicah
        if (System.IO.File.Exists("/Storage/ProfilePics/" + id + rights + ".png"))
        {
            return "/Storage/ProfilePics/"  + id + rights + ".png";
        }
        else
        {
            return "/Storage/ProfilePics/default.png";
        }
    }
}