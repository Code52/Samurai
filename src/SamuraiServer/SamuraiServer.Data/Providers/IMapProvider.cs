using System;

namespace SamuraiServer.Data
{
    public interface IMapProvider
    {
        Map GetRandomMap();
        Map Get(Guid id);
    }
}