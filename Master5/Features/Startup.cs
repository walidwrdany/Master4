using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master5.Features
{
    internal class Startup : IFeature
    {
        private readonly IFeatureFactory _feature;

        public Startup(IFeatureFactory feature)
        {
            _feature = feature;
        }

        public string Id => "00";
        public string Name => "Back To Main Menu";
        public async Task ExecuteAsync()
        {
            await Task.Run(() =>
            {
                _feature.GetAllFeatures().ToList().ForEach(feature =>
                {
                    Console.WriteLine($"\t{feature.Id} | {feature.Name}");
                });
            });
        }
    }
}
