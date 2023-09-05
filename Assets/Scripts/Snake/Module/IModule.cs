using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{

    public interface IModule
    {
        public void Action();
        public void GetHit(int damage);
        public void Die();
        //public void Upgrade();
    }

}
