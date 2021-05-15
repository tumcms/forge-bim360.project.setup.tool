using System;
using System.Collections.Generic;

namespace AdskConstructionCloudBreakdown
{
    public static class Selection
    {
        //Dictionary to map String to Enum 
        public static ProjectTypeEnum SelectProjectType(string input)
        {

            //Dictionary to link String to AccessPermission
            Dictionary<string, ProjectTypeEnum> projectTypeDict =
                new Dictionary<string, ProjectTypeEnum>();
            projectTypeDict.Add("Commercial", ProjectTypeEnum.Commercial);
            projectTypeDict.Add("Convention Center", ProjectTypeEnum.ConventionCenter);
            projectTypeDict.Add("Data Center", ProjectTypeEnum.DataCenter);
            projectTypeDict.Add("Hotel / Motel", ProjectTypeEnum.HotelMotel);
            projectTypeDict.Add("Office", ProjectTypeEnum.Office);
            projectTypeDict.Add("Parking Structure / Garage", ProjectTypeEnum.ParkingStructureGarage);
            projectTypeDict.Add("Performing Arts", ProjectTypeEnum.PerformingArts);
            projectTypeDict.Add("Retail", ProjectTypeEnum.Retail);
            projectTypeDict.Add("Stadium/Arena", ProjectTypeEnum.StadiumArena);
            projectTypeDict.Add("Theme Park", ProjectTypeEnum.ThemePark);
            projectTypeDict.Add("Warehouse", ProjectTypeEnum.Warehouse);
            projectTypeDict.Add("Healthcare", ProjectTypeEnum.Healthcare);
            projectTypeDict.Add("Assisted Living / Nursing Home", ProjectTypeEnum.AssistedLivingNursingHome);
            projectTypeDict.Add("Hospital", ProjectTypeEnum.Hospital);
            projectTypeDict.Add("Medical Laboratory", ProjectTypeEnum.MedicalLaboratory);
            projectTypeDict.Add("Medical Office", ProjectTypeEnum.MedicalOffice);
            projectTypeDict.Add("OutPatient Surgery Center", ProjectTypeEnum.OutPatientSurgeryCenter);
            projectTypeDict.Add("Institutional", ProjectTypeEnum.Institutional);
            projectTypeDict.Add("Court House", ProjectTypeEnum.CourtHouse);
            projectTypeDict.Add("Dormitory", ProjectTypeEnum.Dormitory);
            projectTypeDict.Add("Education Facility", ProjectTypeEnum.EducationFacility);
            projectTypeDict.Add("Government Building", ProjectTypeEnum.GovernmentBuilding);
            projectTypeDict.Add("Library", ProjectTypeEnum.Library);
            projectTypeDict.Add("Military Facility", ProjectTypeEnum.MilitaryFacility);
            projectTypeDict.Add("Museum", ProjectTypeEnum.Museum);
            projectTypeDict.Add("Prison / Correctional Facility", ProjectTypeEnum.PrisonCorrectionalFacility);
            projectTypeDict.Add("Recreation Building", ProjectTypeEnum.RecreationBuilding);
            projectTypeDict.Add("Religious Building", ProjectTypeEnum.ReligiousBuilding);
            projectTypeDict.Add("Research Facility / Laboratory", ProjectTypeEnum.ResearchFacilityLaboratory);
            projectTypeDict.Add("Residential", ProjectTypeEnum.Residential);
            projectTypeDict.Add("Multi-Family Housing", ProjectTypeEnum.MultiFamilyHousing);
            projectTypeDict.Add("Single-Family Housing", ProjectTypeEnum.SingleFamilyHousing);
            projectTypeDict.Add("Infrastructure", ProjectTypeEnum.Infrastructure);
            projectTypeDict.Add("Airport", ProjectTypeEnum.Airport);
            projectTypeDict.Add("Bridge", ProjectTypeEnum.Bridge);
            projectTypeDict.Add("Canal / Waterway", ProjectTypeEnum.CanalWaterway);
            projectTypeDict.Add("Dams / Flood Control / Reservoirs", ProjectTypeEnum.DamsFloodControlReservoirs);
            projectTypeDict.Add("Harbor / River Development", ProjectTypeEnum.HarborRiverDevelopment);
            projectTypeDict.Add("Rail", ProjectTypeEnum.Rail);
            projectTypeDict.Add("Seaport", ProjectTypeEnum.Seaport);
            projectTypeDict.Add("Streets / Roads / Highways", ProjectTypeEnum.StreetsRoadsHighways);
            projectTypeDict.Add("Transportation Building", ProjectTypeEnum.TransportationBuilding);
            projectTypeDict.Add("Tunnel", ProjectTypeEnum.Tunnel);
            projectTypeDict.Add("Waste Water / Sewers", ProjectTypeEnum.WasteWaterSewers);
            projectTypeDict.Add("Water Supply", ProjectTypeEnum.WaterSupply);
            projectTypeDict.Add("Industrial & Energy", ProjectTypeEnum.IndustrialEnergy);
            projectTypeDict.Add("Manufacturing / Factory", ProjectTypeEnum.ManufacturingFactory);
            projectTypeDict.Add("Oil & Gas", ProjectTypeEnum.OilGas);
            projectTypeDict.Add("Plant", ProjectTypeEnum.Plant);
            projectTypeDict.Add("Power Plant", ProjectTypeEnum.PowerPlant);
            projectTypeDict.Add("Solar Far", ProjectTypeEnum.SolarFar);
            projectTypeDict.Add("Utilities", ProjectTypeEnum.Utilities);
            projectTypeDict.Add("Wind Farm", ProjectTypeEnum.WindFarm);
            projectTypeDict.Add("Sample Projects", ProjectTypeEnum.SampleProjects);
            projectTypeDict.Add("Demonstration Project", ProjectTypeEnum.DemonstrationProject);
            projectTypeDict.Add("Template Project", ProjectTypeEnum.TemplateProject);
            projectTypeDict.Add("Training Project", ProjectTypeEnum.TrainingProject);

            try
            {
                return projectTypeDict[input];
            }
            catch
            {
                throw new Exception("Unknown ProjectType found" + input +
                                    "\nsee -> https://forge.autodesk.com/en/docs/bim360/v1/overview/parameters/");
            }
        }


        public static string SelectProjectType(ProjectTypeEnum input)
        {

            //Dictionary to link String to AccessPermission
            Dictionary<ProjectTypeEnum, string> projectTypeDict =
                new Dictionary<ProjectTypeEnum, string>();
            projectTypeDict.Add(ProjectTypeEnum.Commercial,"Commercial");
            projectTypeDict.Add(ProjectTypeEnum.ConventionCenter,"Convention Center");
            projectTypeDict.Add(ProjectTypeEnum.DataCenter,"Data Center");
            projectTypeDict.Add(ProjectTypeEnum.HotelMotel,"Hotel / Motel");
            projectTypeDict.Add(ProjectTypeEnum.Office, "Office");
            projectTypeDict.Add(ProjectTypeEnum.ParkingStructureGarage,"Parking Structure / Garage" );
            projectTypeDict.Add(ProjectTypeEnum.PerformingArts,"Performing Arts");
            projectTypeDict.Add(ProjectTypeEnum.Retail,"Retail");
            projectTypeDict.Add(ProjectTypeEnum.StadiumArena,"Stadium/Arena");
            projectTypeDict.Add(ProjectTypeEnum.ThemePark, "Theme Park");
            projectTypeDict.Add(ProjectTypeEnum.Warehouse,"Warehouse");
            projectTypeDict.Add(ProjectTypeEnum.Healthcare,"Healthcare" );
            projectTypeDict.Add(ProjectTypeEnum.AssistedLivingNursingHome,"Assisted Living / Nursing Home");
            projectTypeDict.Add(ProjectTypeEnum.Hospital,"Hospital");
            projectTypeDict.Add(ProjectTypeEnum.MedicalLaboratory,"Medical Laboratory");
            projectTypeDict.Add(ProjectTypeEnum.MedicalOffice,"Medical Office");
            projectTypeDict.Add(ProjectTypeEnum.OutPatientSurgeryCenter,"OutPatient Surgery Center");
            projectTypeDict.Add(ProjectTypeEnum.Institutional,"Institutional" );
            projectTypeDict.Add(ProjectTypeEnum.CourtHouse,"Court House");
            projectTypeDict.Add(ProjectTypeEnum.Dormitory,"Dormitory");
            projectTypeDict.Add(ProjectTypeEnum.EducationFacility,"Education Facility");
            projectTypeDict.Add(ProjectTypeEnum.GovernmentBuilding,"Government Building");
            projectTypeDict.Add(ProjectTypeEnum.Library,"Library");
            projectTypeDict.Add(ProjectTypeEnum.MilitaryFacility, "Military Facility");
            projectTypeDict.Add(ProjectTypeEnum.Museum, "Museum");
            projectTypeDict.Add(ProjectTypeEnum.PrisonCorrectionalFacility, "Prison / Correctional Facility");
            projectTypeDict.Add(ProjectTypeEnum.RecreationBuilding,"Recreation Building");
            projectTypeDict.Add(ProjectTypeEnum.ReligiousBuilding,"Religious Building");
            projectTypeDict.Add(ProjectTypeEnum.ResearchFacilityLaboratory,"Research Facility / Laboratory");
            projectTypeDict.Add(ProjectTypeEnum.Residential,"Residential");
            projectTypeDict.Add(ProjectTypeEnum.MultiFamilyHousing, "Multi-Family Housing");
            projectTypeDict.Add(ProjectTypeEnum.SingleFamilyHousing,"Single-Family Housing");
            projectTypeDict.Add(ProjectTypeEnum.Infrastructure,"Infrastructure");
            projectTypeDict.Add(ProjectTypeEnum.Airport,"Airport" );
            projectTypeDict.Add(ProjectTypeEnum.Bridge,"Bridge");
            projectTypeDict.Add(ProjectTypeEnum.CanalWaterway,"Canal / Waterway");
            projectTypeDict.Add(ProjectTypeEnum.DamsFloodControlReservoirs,"Dams / Flood Control / Reservoirs");
            projectTypeDict.Add(ProjectTypeEnum.HarborRiverDevelopment,"Harbor / River Development");
            projectTypeDict.Add(ProjectTypeEnum.Rail,"Rail");
            projectTypeDict.Add(ProjectTypeEnum.Seaport,"Seaport");
            projectTypeDict.Add(ProjectTypeEnum.StreetsRoadsHighways,"Streets / Roads / Highways");
            projectTypeDict.Add(ProjectTypeEnum.TransportationBuilding,"Transportation Building");
            projectTypeDict.Add(ProjectTypeEnum.Tunnel,"Tunnel" );
            projectTypeDict.Add(ProjectTypeEnum.WasteWaterSewers,"Waste Water / Sewers");
            projectTypeDict.Add(ProjectTypeEnum.WaterSupply,"Water Supply");
            projectTypeDict.Add(ProjectTypeEnum.IndustrialEnergy,"Industrial & Energy");
            projectTypeDict.Add(ProjectTypeEnum.ManufacturingFactory,"Manufacturing / Factory");
            projectTypeDict.Add(ProjectTypeEnum.OilGas,"Oil & Gas");
            projectTypeDict.Add(ProjectTypeEnum.Plant,"Plant");
            projectTypeDict.Add(ProjectTypeEnum.PowerPlant,"Power Plant");
            projectTypeDict.Add(ProjectTypeEnum.SolarFar,"Solar Far" );
            projectTypeDict.Add(ProjectTypeEnum.Utilities,"Utilities");
            projectTypeDict.Add(ProjectTypeEnum.WindFarm,"Wind Farm");
            projectTypeDict.Add(ProjectTypeEnum.SampleProjects,"Sample Projects");
            projectTypeDict.Add(ProjectTypeEnum.DemonstrationProject,"Demonstration Project");
            projectTypeDict.Add(ProjectTypeEnum.TemplateProject,"Template Project");
            projectTypeDict.Add(ProjectTypeEnum.TrainingProject,"Training Project");

            return projectTypeDict[input];

        }

    }
}