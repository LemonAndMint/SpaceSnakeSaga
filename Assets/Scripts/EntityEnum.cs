namespace EntityEnum
{

    public enum Entity { Player, Enemy }

    public static class EntityUtil{

        /// <summary>
        /// Entity enumdan string haline çevirir.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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

    }

}
