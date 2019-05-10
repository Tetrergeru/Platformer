using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer.Physics
{
    public struct PhysicalMaterial
    {
        /// <summary>
        /// Плотность (если всё получится, то её можно будет брать из физических табличек)
        /// </summary>
        public double Density { get; set; }

        /// <summary>
        /// Коэффициент упругости (может принимать значение от 0 до 1)
        /// </summary>
        public double Restoring { get; set; }

        /// <summary>
        /// Коэффициент вязкости (может принимать значение от 0 до 1)
        /// </summary>
        public double Viscosity { get; set; }

        /// <summary>
        /// Коэффициент поглащения импулься (может принимать значение от 0 до 1)
        /// </summary>
        public double Absorption { get; set; }

        /// <summary>
        /// Коэффициент трения (может принимать значение от 0 до 1)
        /// </summary>
        public double Friction { get; set; }

        /// <summary>
        /// Определяет, управляется ли перемещение данного тела физическим движком
        /// </summary>
        public bool MovementRecipient { get; set; }

        /// <summary>
        /// Определяет, влияет ли это объект на другие, во время физического взаимодействия
        /// </summary>
        public bool MovementEmitter { get; set; }
    }
}
