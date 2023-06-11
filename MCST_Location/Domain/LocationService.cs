using System;
using MCST_ControllerInterfaceLayer.Interfaces;
using MCST_Location.Data;
using MCST_Location.Data.DTOs;
using MCST_Models;

namespace MCST_Location.Domain
{
	public class LocationService
	{
		private readonly LocationRepository repo;
        private readonly IWsService clientWsService;

        public LocationService(LocationRepository repo, IWsService _clientWsService)
        {
            this.repo = repo;
            this.clientWsService = _clientWsService;
        }

        public bool NewLocation(Location location)
        {
            if (location.IsValid())
            {
                clientWsService.NewLocationOverWS(location);
                repo.InsertLocation(new LocationDTO(location.ComputerId, new CoordinatesDTO(location.Coordinates.X, location.Coordinates.Y, location.Coordinates.Z), location.Dimension, location.CreatedOn));
                return true;
            } else
            {
                return false;
            }
        }

        public Location GetLocationByComputerId(int computerId)
        {
            LocationDTO locationDTO = repo.GetLocationByComputerId(computerId);
            Location location = new Location
            {
                ComputerId = locationDTO.ComputerId,
                Coordinates = new Coordinates(locationDTO.Coordinates.X, locationDTO.Coordinates.Y, locationDTO.Coordinates.Z),
                Dimension = locationDTO.Dimension
            };
            location.OverrideCreatedOn(locationDTO.CreatedOn);

            return location;
        }

        
    }
}

