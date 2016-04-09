using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Hexocracy
{
    public class GS
    {
        #region Static
        private const string POST_CTOR_METHOD = "r_post_ctor";
        private static GS instance;

        public static Type[] GetHexocracyTypes()
        {
            return instance.types;
        }

        public static void Initialize()
        {
            if (instance != null) return;

            instance = new GS();
            instance.InitializeAllServices();
            instance.PostInitializeAllServices();
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
        #endregion

        #region Instance
        private Dictionary<int, object> services;
        private Type[] types;

        private GS()
        {
            services = new Dictionary<int, object>();
        }

        private void InitializeAllServices()
        {
            types = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.Namespace.Contains(ProjectInfo.MAIN_NAMESPACE)).ToArray();
            
            var serviceTypes = types.Where(type => type.IsDefined(typeof(GameServiceAttribute), true));
            foreach (var serviceType in serviceTypes)
            {
                var constructor = serviceType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
                services[serviceType.GetHashCode()] = constructor.Invoke(new Type[0]);
            }
        }

        private void PostInitializeAllServices()
        {
            foreach (var service in services.Values)
            {
                var method = service.GetType().GetMethod(POST_CTOR_METHOD, BindingFlags.NonPublic | BindingFlags.Instance);
                if (method != null)
                {
                    method.Invoke(service, null);
                }
            }
        }

        #endregion
    }
}
