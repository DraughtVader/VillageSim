using System;
using VillageSim.Resources;

namespace VillageSim.Buildings
{
    public interface IBuilding
    {
        string Name { get; }
        string Description { get; }
        ResourceAmount[] ResourcesRequired { get; }
       // Action ResourcesChanged { get; }
    }
}