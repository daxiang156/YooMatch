using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
namespace ET
{
    public class UIGoldEffectItem
    {
        private Vector3[] path;
        public GameObject gameObject;
        private bool CanMoving = false;
        private Action aniFinish;
        public float aniTime = 0.5f;
        public UIGoldEffectItem(GameObject go,Action aniFinish)
        {
            this.gameObject = go;
            this.aniFinish = aniFinish;
        }
        /// <summary>
        /// _segmentNum 显示个数
        /// </summary>
        public void SetPath(Vector3 p0,Vector3 p1,Vector3 p2,int segmentNum )
        {
           // this._segmentNum = segmentNum;
            // for (int i = 0; i < _segmentNum ; i++)
            // {
            //     Vector3 tempPoint = BezierUtility.CalculateBezierPoint(p0, p1, p2, (i / 50.0f));
            //     this.path.Add(tempPoint);
            // }


            path = new Vector3[segmentNum];
            for (int i = 1; i <= segmentNum; i++)
            {
                float t = i / (float)segmentNum;
                Vector3 pixel = BezierUtility.CalculateBezierPoint( p0,
                    p1, p2,t);
                this.path[i - 1] = pixel;
            }
            
            
            
            CanMoving = true;
            this.Move();
        }
        public void UpDate()
        {
            if (!this.CanMoving)
                return;
            
            this.Move();
        }
        

        // /// <summary>
        // /// 移动的速度
        // /// </summary>
        // private float _speed = 2000;
        // /// <summary>
        // /// 所有的路径点的索引值
        // /// </summary>
        // private int _index = 0;
        //
        // /// <summary>
        // /// 是否完成移动
        // /// </summary>
        // private bool _isMoveComplete = false;
        // public int _segmentNum;
        /// <summary>
        ///  移动
        /// </summary>
        /// <param name="moveComplete">移动完成的回调</param>
        // public void Move()
        // {
        //     if (this.path == null|| this.path.Count==0)
        //     {
        //         Debug.LogError("没有初始化贝赛尔曲线的所有的路径点");
        //         return;
        //     }
        //     
        //     if(!_isMoveComplete)
        //     {
        //         if (Vector3.Distance(gameObject.transform.position, this.path[_index]) <= 0.1f)
        //         {
        //             _index++;
        //             if (_index > _segmentNum - 1)
        //             {
        //                 _index = _segmentNum - 1;
        //                 if (aniFinish != null)
        //                 {
        //                     aniFinish();
        //
        //                 }
        //                 _isMoveComplete = true;
        //             }
        //         }
        //         else
        //         {
        //             gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, path[_index], _speed * Time.deltaTime);
        //         }
        //
        //         this.gameObject.transform.DOPath(this.path,2);
        //     }
        // }
        public void Move()
        {
            if (this.gameObject == null)
            {
                Log.Warning("gameObject == null");
                return;
            }
            var tweenScale3  = this.gameObject.transform.DOPath(this.path,aniTime);
            tweenScale3.onComplete = () =>
            {
                this.aniFinish?.Invoke();
            };
        }
    }
}