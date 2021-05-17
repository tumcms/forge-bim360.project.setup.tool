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

            //reverse Dict
            var projectTypeDict_reverse = new Dictionary<ProjectTypeEnum,string>();
            foreach (var entry in projectTypeDict)
            {
                if (!projectTypeDict_reverse.ContainsKey(entry.Value))
                    projectTypeDict_reverse.Add(entry.Value, entry.Key);
            }

            return projectTypeDict_reverse[input];

        }

        public static TradeEnum SelectTrade(string input)
        {
            Dictionary<string, TradeEnum> tradeDict =
                new Dictionary<string, TradeEnum>();
            tradeDict.Add("Architecture", TradeEnum.Architecture);
            tradeDict.Add("Communications", TradeEnum.Communications);
            tradeDict.Add("Communications | Data", TradeEnum.CommunicationsData);
            tradeDict.Add("Concrete", TradeEnum.Concrete);
            tradeDict.Add("Concrete | Cast-in-Place", TradeEnum.ConcreteCastinPlace);
            tradeDict.Add("Concrete | Precast", TradeEnum.ConcretePrecast);
            tradeDict.Add("Construction Management", TradeEnum.ConstructionManagement);
            tradeDict.Add("Conveying Equipment", TradeEnum.ConveyingEquipment);
            tradeDict.Add("Conveying Equipment | Elevators", TradeEnum.ConveyingEquipmentElevators);
            tradeDict.Add("Demolition", TradeEnum.Demolition);
            tradeDict.Add("Earthwork", TradeEnum.Earthwork);
            tradeDict.Add("Earthwork | Site Excavation & Grading", TradeEnum.EarthworkSiteExcavationGrading);
            tradeDict.Add("Electrical", TradeEnum.Electrical);
            tradeDict.Add("Electrical Power Generation", TradeEnum.ElectricalPowerGeneration);
            tradeDict.Add("Electronic Safety & Security", TradeEnum.ElectronicSafetySecurity);
            tradeDict.Add("Equipment", TradeEnum.Equipment);
            tradeDict.Add("Equipment | Kitchen Appliances", TradeEnum.EquipmentKitchenAppliances);
            tradeDict.Add("Exterior Improvements", TradeEnum.ExteriorImprovements);
            tradeDict.Add("Exterior | Fences & Gates", TradeEnum.ExteriorFencesGates);
            tradeDict.Add("Exterior | Landscaping", TradeEnum.ExteriorLandscaping);
            tradeDict.Add("Exterior | Irrigation", TradeEnum.ExteriorIrrigation);
            tradeDict.Add("Finishes", TradeEnum.Finishes);
            tradeDict.Add("Finishes | Carpeting", TradeEnum.FinishesCarpeting);
            tradeDict.Add("Finishes | Ceiling", TradeEnum.FinishesCeiling);
            tradeDict.Add("Finishes | Drywall", TradeEnum.FinishesDrywall);
            tradeDict.Add("Finishes | Flooring", TradeEnum.FinishesFlooring);
            tradeDict.Add("Finishes | Painting & Coating", TradeEnum.FinishesPaintingCoating);
            tradeDict.Add("Finishes | Tile", TradeEnum.FinishesTile);
            tradeDict.Add("Fire Suppression", TradeEnum.FireSuppression);
            tradeDict.Add("Furnishings", TradeEnum.Furnishings);
            tradeDict.Add("Furnishings | Casework & Cabinets", TradeEnum.FurnishingsCaseworkCabinets);
            tradeDict.Add("Furnishings | Countertops", TradeEnum.FurnishingsCountertops);
            tradeDict.Add("Furnishings | Window Treatments", TradeEnum.FurnishingsWindowTreatments);
            tradeDict.Add("General Contractor", TradeEnum.GeneralContractor);
            tradeDict.Add("HVAC Heating, Ventilating, & Air Conditioning", TradeEnum.HVACHeatingVentilatingAirConditioning);
            tradeDict.Add("Industry-Specific Manufacturing Processing", TradeEnum.IndustrySpecificManufacturingProcessing);
            tradeDict.Add("Integrated Automation", TradeEnum.IntegratedAutomation);
            tradeDict.Add("Masonry", TradeEnum.Masonry);
            tradeDict.Add("Material Processing & Handling Equipment", TradeEnum.MaterialProcessingHandlingEquipment);
            tradeDict.Add("Metals", TradeEnum.Metals);
            tradeDict.Add("Metals | Structural Steel / Framing", TradeEnum.MetalsStructuralSteelFraming);
            tradeDict.Add("Moisture Protection", TradeEnum.MoistureProtection);
            tradeDict.Add("Moisture Protection | Roofing", TradeEnum.MoistureProtectionRoofing);
            tradeDict.Add("Moisture Protection | Waterproofing", TradeEnum.MoistureProtectionWaterproofing);
            tradeDict.Add("Openings", TradeEnum.Openings);
            tradeDict.Add("Openings | Doors & Frames", TradeEnum.OpeningsDoorsFrames);
            tradeDict.Add("Openings | Entrances & Storefronts", TradeEnum.OpeningsEntrancesStorefronts);
            tradeDict.Add("Openings | Glazing", TradeEnum.OpeningsGlazing);
            tradeDict.Add("Openings | Roof Windows & Skylights", TradeEnum.OpeningsRoofWindowsSkylights);
            tradeDict.Add("Openings | Windows", TradeEnum.OpeningsWindows);
            tradeDict.Add("Owner", TradeEnum.Owner);
            tradeDict.Add("Plumbing", TradeEnum.Plumbing);
            tradeDict.Add("Pollution & Waste Control Equipment", TradeEnum.PollutionWasteControlEquipment);
            tradeDict.Add("Process Gas & Liquid Handling, Purification, & Storage Equipment", TradeEnum.ProcessGasLiquidHandlingPurificationStorageEquipment);
            tradeDict.Add("Process Heating, Cooling, & Drying Equipment", TradeEnum.ProcessHeatingCoolingDryingEquipment);
            tradeDict.Add("Process Integration", TradeEnum.ProcessIntegration);
            tradeDict.Add("Process Integration | Piping", TradeEnum.ProcessIntegrationPiping);
            tradeDict.Add("Special Construction", TradeEnum.SpecialConstruction);
            tradeDict.Add("Specialties", TradeEnum.Specialties);
            tradeDict.Add("Specialties | Signage", TradeEnum.SpecialtiesSignage);
            tradeDict.Add("Utilities", TradeEnum.Utilities);
            tradeDict.Add("Water & Wastewater Equipment", TradeEnum.WaterWastewaterEquipment);
            tradeDict.Add("Waterway & Marine Construction", TradeEnum.WaterwayMarineConstruction);
            tradeDict.Add("Wood & Plastics", TradeEnum.WoodPlastics);
            tradeDict.Add("Wood & Plastics | Millwork", TradeEnum.WoodPlasticsMillwork);
            tradeDict.Add("Wood & Plastics | Rough Carpentry", TradeEnum.WoodPlasticsRoughCarpentry);

            try
            {
                return tradeDict[input];
            }
            catch
            {
                throw new Exception("Unknown Trade found" + input +
                                    "\nsee -> https://forge.autodesk.com/en/docs/bim360/v1/overview/parameters/");
            }
        }

        public static string SelectTrade(TradeEnum input)
        {
            Dictionary<string, TradeEnum> tradeDict =
               new Dictionary<string, TradeEnum>();
            tradeDict.Add("Architecture", TradeEnum.Architecture);
            tradeDict.Add("Communications", TradeEnum.Communications);
            tradeDict.Add("Communications | Data", TradeEnum.CommunicationsData);
            tradeDict.Add("Concrete", TradeEnum.Concrete);
            tradeDict.Add("Concrete | Cast-in-Place", TradeEnum.ConcreteCastinPlace);
            tradeDict.Add("Concrete | Precast", TradeEnum.ConcretePrecast);
            tradeDict.Add("Construction Management", TradeEnum.ConstructionManagement);
            tradeDict.Add("Conveying Equipment", TradeEnum.ConveyingEquipment);
            tradeDict.Add("Conveying Equipment | Elevators", TradeEnum.ConveyingEquipmentElevators);
            tradeDict.Add("Demolition", TradeEnum.Demolition);
            tradeDict.Add("Earthwork", TradeEnum.Earthwork);
            tradeDict.Add("Earthwork | Site Excavation & Grading", TradeEnum.EarthworkSiteExcavationGrading);
            tradeDict.Add("Electrical", TradeEnum.Electrical);
            tradeDict.Add("Electrical Power Generation", TradeEnum.ElectricalPowerGeneration);
            tradeDict.Add("Electronic Safety & Security", TradeEnum.ElectronicSafetySecurity);
            tradeDict.Add("Equipment", TradeEnum.Equipment);
            tradeDict.Add("Equipment | Kitchen Appliances", TradeEnum.EquipmentKitchenAppliances);
            tradeDict.Add("Exterior Improvements", TradeEnum.ExteriorImprovements);
            tradeDict.Add("Exterior | Fences & Gates", TradeEnum.ExteriorFencesGates);
            tradeDict.Add("Exterior | Landscaping", TradeEnum.ExteriorLandscaping);
            tradeDict.Add("Exterior | Irrigation", TradeEnum.ExteriorIrrigation);
            tradeDict.Add("Finishes", TradeEnum.Finishes);
            tradeDict.Add("Finishes | Carpeting", TradeEnum.FinishesCarpeting);
            tradeDict.Add("Finishes | Ceiling", TradeEnum.FinishesCeiling);
            tradeDict.Add("Finishes | Drywall", TradeEnum.FinishesDrywall);
            tradeDict.Add("Finishes | Flooring", TradeEnum.FinishesFlooring);
            tradeDict.Add("Finishes | Painting & Coating", TradeEnum.FinishesPaintingCoating);
            tradeDict.Add("Finishes | Tile", TradeEnum.FinishesTile);
            tradeDict.Add("Fire Suppression", TradeEnum.FireSuppression);
            tradeDict.Add("Furnishings", TradeEnum.Furnishings);
            tradeDict.Add("Furnishings | Casework & Cabinets", TradeEnum.FurnishingsCaseworkCabinets);
            tradeDict.Add("Furnishings | Countertops", TradeEnum.FurnishingsCountertops);
            tradeDict.Add("Furnishings | Window Treatments", TradeEnum.FurnishingsWindowTreatments);
            tradeDict.Add("General Contractor", TradeEnum.GeneralContractor);
            tradeDict.Add("HVAC Heating, Ventilating, & Air Conditioning", TradeEnum.HVACHeatingVentilatingAirConditioning);
            tradeDict.Add("Industry-Specific Manufacturing Processing", TradeEnum.IndustrySpecificManufacturingProcessing);
            tradeDict.Add("Integrated Automation", TradeEnum.IntegratedAutomation);
            tradeDict.Add("Masonry", TradeEnum.Masonry);
            tradeDict.Add("Material Processing & Handling Equipment", TradeEnum.MaterialProcessingHandlingEquipment);
            tradeDict.Add("Metals", TradeEnum.Metals);
            tradeDict.Add("Metals | Structural Steel / Framing", TradeEnum.MetalsStructuralSteelFraming);
            tradeDict.Add("Moisture Protection", TradeEnum.MoistureProtection);
            tradeDict.Add("Moisture Protection | Roofing", TradeEnum.MoistureProtectionRoofing);
            tradeDict.Add("Moisture Protection | Waterproofing", TradeEnum.MoistureProtectionWaterproofing);
            tradeDict.Add("Openings", TradeEnum.Openings);
            tradeDict.Add("Openings | Doors & Frames", TradeEnum.OpeningsDoorsFrames);
            tradeDict.Add("Openings | Entrances & Storefronts", TradeEnum.OpeningsEntrancesStorefronts);
            tradeDict.Add("Openings | Glazing", TradeEnum.OpeningsGlazing);
            tradeDict.Add("Openings | Roof Windows & Skylights", TradeEnum.OpeningsRoofWindowsSkylights);
            tradeDict.Add("Openings | Windows", TradeEnum.OpeningsWindows);
            tradeDict.Add("Owner", TradeEnum.Owner);
            tradeDict.Add("Plumbing", TradeEnum.Plumbing);
            tradeDict.Add("Pollution & Waste Control Equipment", TradeEnum.PollutionWasteControlEquipment);
            tradeDict.Add("Process Gas & Liquid Handling, Purification, & Storage Equipment", TradeEnum.ProcessGasLiquidHandlingPurificationStorageEquipment);
            tradeDict.Add("Process Heating, Cooling, & Drying Equipment", TradeEnum.ProcessHeatingCoolingDryingEquipment);
            tradeDict.Add("Process Integration", TradeEnum.ProcessIntegration);
            tradeDict.Add("Process Integration | Piping", TradeEnum.ProcessIntegrationPiping);
            tradeDict.Add("Special Construction", TradeEnum.SpecialConstruction);
            tradeDict.Add("Specialties", TradeEnum.Specialties);
            tradeDict.Add("Specialties | Signage", TradeEnum.SpecialtiesSignage);
            tradeDict.Add("Utilities", TradeEnum.Utilities);
            tradeDict.Add("Water & Wastewater Equipment", TradeEnum.WaterWastewaterEquipment);
            tradeDict.Add("Waterway & Marine Construction", TradeEnum.WaterwayMarineConstruction);
            tradeDict.Add("Wood & Plastics", TradeEnum.WoodPlastics);
            tradeDict.Add("Wood & Plastics | Millwork", TradeEnum.WoodPlasticsMillwork);
            tradeDict.Add("Wood & Plastics | Rough Carpentry", TradeEnum.WoodPlasticsRoughCarpentry);


            var tradeDict_reverse = new Dictionary<TradeEnum, string>();
            foreach (var entry in tradeDict)
            {
                if (!tradeDict_reverse.ContainsKey(entry.Value))
                    tradeDict_reverse.Add(entry.Value, entry.Key);
            }

            return tradeDict_reverse[input];
            
            
        }
    }
}