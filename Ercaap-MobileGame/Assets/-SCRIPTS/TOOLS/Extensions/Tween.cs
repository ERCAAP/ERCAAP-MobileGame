using System.Collections;
using UnityEngine;

namespace LunaGames.Extentions
{
    public static class Tween
    {
        public static IEnumerator LinearTween(this Transform self, Transform target, float time)
        {
            Vector3 initPos = self.position;
            self.SetParent(null);
            float timer = 0;
            while (timer < 1)
            {
                if (!Application.isPlaying || self == null) break;
                self.position = Vector3.Slerp(initPos, target.position, timer);
                timer += Time.deltaTime / time;
                yield return null;
            }
            self.SetParent(target);
        }

        public static void Follow(this Transform self, Transform target, float speed, bool lookAt)
        {
            self.position = Vector3.MoveTowards(self.position, target.position, Time.deltaTime * speed);
            if (lookAt) self.LookAt(target);
        }
    }
}
