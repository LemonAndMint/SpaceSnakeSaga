using System;

namespace EntityEnum
{

    public enum Entity { None, Player, Enemy }

    public static class EntityUtil{

        public static string EnumToString(Entity entity){

            switch (entity)
            {
                case Entity.Player:
                    return "Player";
                
                case Entity.Enemy:
                    return "Enemy";

                default:
                    return null;
                    
            }


        }

        public static Entity StringToEnum(string entityName){


            switch (entityName)
            {
                
                case "Player":
                    return Entity.Player;
                
                case "Enemy":
                    return Entity.Enemy;

                default:
                    return Entity.None;

            }


        }

    }

}
