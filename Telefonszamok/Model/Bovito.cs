using Model;
public static class Bovito
{
    public static string Telefonszamok(this enSzemely s)
    {
        string str = "";
        foreach (var x in s.enTelefonszamok)
        {
            str += x.Szam;
            if (x != s.enTelefonszamok.Last())
                str += ", ";
        }
        return str;
    }
}