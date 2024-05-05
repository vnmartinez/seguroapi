public class PessoaConsulta
{
    public int Age {  get; set; }
    public required string Gender { get; set; }
    public int DrivingExperience { get; set; }
    public required string Education { get; set; }
    public required string Income { get; set; }
    public int VehicleYear { get; set; }
    public required string VehicleType { get; set; }
    public int AnnualMileage {  get; set; } 

    public String GetAgeRange(){
        if (Utils.IsBetween(Age, 16,25))
            return "16-25";
        if (Utils.IsBetween(Age, 26,39))
            return "26-39";
        if (Utils.IsBetween(Age, 40,64))
            return "40-64";
        if (Age < 16)
            return "<16";
        else
            return "65+";
    }
    public string GetDrivingExperienceRange(){
        if (Utils.IsBetween(DrivingExperience, 0, 9))
            return "0-9";
        if (Utils.IsBetween(DrivingExperience, 10, 19))
            return "10-19";
        if (Utils.IsBetween(DrivingExperience, 20, 29))
            return "20-29";
        else
            return "30y+";
    }

    public String GetVehicleYear()
    {
        if (Utils.GreaterThan2015(VehicleYear))
            return "after 2015";
        else
            return "before 2015";
    }
}