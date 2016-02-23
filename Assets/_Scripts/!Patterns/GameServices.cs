using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hexocracy
{
    public class GameServices
    {
        private static GameServices instance;

        public static void Initialize()
        {
            if (instance != null) return;

            instance = new GameServices();
            instance.InitializeAllServices();
        }

        public static T Get<T>() where T : class
        {
            var key = typeof(T).GetHashCode();
            if (instance.services.ContainsKey(key))
            {
                return (T)instance.services[key];
            }
            else
            {
                return null;
            }
        }


        private const string MAIN_NAMESPACE = "Hexocracy";

        private Dictionary<int, object> services;

        private GameServices()
        {
            services = new Dictionary<int, object>();
        }

        private void InitializeAllServices()
        {
            var serviceTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.Namespace.Contains(MAIN_NAMESPACE) && type.IsDefined(typeof(GameServiceAttribute), true));
            foreach (var serviceType in serviceTypes)
            {
                var constructor = serviceType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
                services[serviceType.GetHashCode()] = constructor.Invoke(new Type[0]);
            }
        }
    }
}
