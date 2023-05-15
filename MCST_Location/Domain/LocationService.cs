using System;
using MCST_Location.Data;
using MCST_Location.Data.DTOs;
using MCST_Location.Domain.Models;

namespace MCST_Location.Domain
{
	public class LocationService
	{
		private readonly LocationRepository repo;
        private readonly ILocationController controller;

        public LocationService(LocationRepository repo, ILocationController controller)
        {
            this.repo = repo;
            this.controller = controller;
        }

        public bool NewLocation(Location location)
        {
            if (location.IsValid())
            {
                controller.NewLocationOverWS(location);
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

