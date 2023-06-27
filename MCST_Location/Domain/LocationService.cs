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

        public async Task<bool> NewLocation(Location location)
        {
            if (location.IsValid())
            {
                await clientWsService.NewLocationOverWS(location);
                await repo.InsertLocation(new LocationDTO(location.ComputerId, new CoordinatesDTO(location.Coordinates.X, location.Coordinates.Y, location.Coordinates.Z), location.Dimension, location.CreatedOn));
                return true;
            } else
            {
                return false;
            }
        }

        public async Task<Location> GetLocationByComputerId(int computerId)
        {
            LocationDTO locationDTO = await repo.GetLocationByComputerId(computerId);
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

