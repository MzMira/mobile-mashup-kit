using UnityEngine;

namespace PlantsVsBloons.Bloons
{
    public class RedBloon : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.Red;
            baseHealth = 1f;
            speed = 2f;
            damage = 1;
            bloonColor = Color.red;
        }
    }

    public class BlueBloon : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.Blue;
            baseHealth = 1f;
            speed = 2.5f;
            damage = 1;
            bloonColor = Color.blue;
        }
    }

    public class GreenBloon : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.Green;
            baseHealth = 1f;
            speed = 3f;
            damage = 1;
            bloonColor = Color.green;
        }
    }

    public class YellowBloon : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.Yellow;
            baseHealth = 1f;
            speed = 3.5f;
            damage = 1;
            bloonColor = Color.yellow;
        }
    }

    public class PinkBloon : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.Pink;
            baseHealth = 1f;
            speed = 4f;
            damage = 1;
            bloonColor = new Color(1f, 0.4f, 0.7f);
        }
    }

    public class BlackBloon : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.Black;
            baseHealth = 1f;
            speed = 2.5f;
            damage = 1;
            bloonColor = Color.black;
        }
    }

    public class WhiteBloon : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.White;
            baseHealth = 1f;
            speed = 3f;
            damage = 1;
            bloonColor = Color.white;
        }
    }

    public class RainbowBloon : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.Rainbow;
            baseHealth = 1f;
            speed = 2.5f;
            damage = 1;
            bloonColor = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public class CeramicBloon : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.Ceramic;
            baseHealth = 10f;
            speed = 2.5f;
            damage = 1;
            bloonColor = new Color(0.6f, 0.3f, 0.1f);
        }
    }

    public class MOAB : Bloon
    {
        protected override void Awake()
        {
            base.Awake();
            bloonType = BloonType.MOAB;
            baseHealth = 200f;
            speed = 1.5f;
            damage = 1;
            bloonColor = new Color(0.2f, 0.4f, 0.8f);
        }
    }
}
