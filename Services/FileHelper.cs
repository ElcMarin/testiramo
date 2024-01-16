namespace maturitetna.Services;

public class FileHelper
{
    static public string GetProfilePicture( int id, char rights)
    {
        //kak nardim poizvedbo tule notr da vidim v kateri tabeli je pac po pravicah
        if (System.IO.File.Exists("/Storage/ProfilePics/" + rights + id + ".png"))
        {
            return "/Storage/ProfilePics/" + rights + id + ".png";
        }
        else
        {
            return "/Storage/ProfilePics/default.png";
        }
    }
}