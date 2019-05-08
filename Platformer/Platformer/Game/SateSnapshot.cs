using System.Collections.Generic;
using System.Drawing;
using Platformer.GUI;

namespace Platformer.Game
{
    public struct GameObject
    {
        public HitBox body;
        public string texture;
        public double drawPriority;
    }
    
    public class StateSnapshot
    {
        public GameObject player;
        public List<GameObject> entities;
        public bool gameIsOver;
        public Color currentBackgroundColor;
    }
}