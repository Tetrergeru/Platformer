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
        /// Коэффициент упругости (по идеи должен изменяться от 0 до 1)
        /// </summary>
        public double Restoring { get; set; }

        /// <summary>
        /// Коэффициент вязкости (тоже предполагается от 0 до 1)
        /// </summary>
        public double Viscosity { get; set; }

        /// <summary>
        /// Коэффициент поглащения импулься (то 0 до 1)
        /// </summary>
        public double Absorption { get; set; }

        /// <summary>
        /// Коэффициент трения (от 0 до 1)
        /// </summary>
        public double Friction { get; set; }

        /// <summary>
        /// Определяет, упрявляется ли перемещение данного тела физическим движком
        /// </summary>
        public bool MovementRecipient { get; set; }

        /// <summary>
        /// Определяет, влияет ли физика на перемещение данного объекта
        /// </summary>
        public bool MovementEmitter { get; set; }
    }
}
