﻿using Game.Components;
using Game.Components.Colliders;
using Game.Components.Transform;
using Game.Components.Utilities;
using TinyEcs;

namespace Game
{
    public class EnemyBullets
    {
        public int Length;
        public RoData<Entity> Entities;
        public RoData<Position> Positions;
        public RoData<Circle> Colliders;
        public EnemyTag EnemyTag;
        public BulletTag BulletTag;
    }

    public class PlayerBullets
    {
        public int Length;
        public RoData<Entity> Entities;
        public RoData<Position> Positions;
        public RoData<Circle> Colliders;
        public RoData<ParentEntity> Parents;
        public PlayerTag PlayerTag;
        public BulletTag BulletTag;
    }

    public class Players
    {
        public int Length;
        public RoData<Entity> Entities;
        public RoData<Position> Positions;
        public RoData<Circle> Colliders;
        public PlayerTag PlayerTag;
        public ShipTag ShipTag;
    }

    public class Enemies
    {
        public int Length;
        public RoData<Entity> Entities;
        public RoData<Position> Positions;
        public RoData<Circle> Colliders;
        public EnemyTag EnemyTag;
        public ShipTag ShipTag;
    }
}