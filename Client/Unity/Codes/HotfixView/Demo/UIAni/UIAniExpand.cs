using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public enum DirectionType
    {
        Up = 0,
        Down,
        Left,
        Right,
    }

    public class UIAniExpand
    {
        /// <summary>
        /// 动画类型，0 收起 ，1 展开
        /// </summary>
        private int curAniType = 1;
        /// <summary>
        /// 上下左右操作对象 一定要传4个对象，没有传null
        /// </summary>
        private List<GameObject> gameObjectlist;
        private List<AniItem> aniList = new List<AniItem>();
        public UIAniExpand(List<GameObject> gameObjectlist)
        {
            this.gameObjectlist = gameObjectlist;
            this.InitUI();
            this.AddListner();
        }

        private void InitUI()
        {
            for (int i = 0; i < this.gameObjectlist.Count; i++)
            {
                if(this.gameObjectlist[i]== null)
                    continue;
                AniItem aniItem = new AniItem(this.gameObjectlist[i],(DirectionType)i);
                aniList.Add(aniItem);
            }
        }

        private void AddListner()
        {
            EventDispatcher.AddObserver(this, ETEventName.UIAniType, (object[] paras) =>
            {
                int aniType = (int) paras[0];
                PlayAni(aniType);
                return false;
            }, null);
        }

        public void Dispose()
        {
            EventDispatcher.RemoveObserver(this,ETEventName.UIAniType,null);
        }

        private void PlayAni(int aniType)
        {
            if (this.curAniType == aniType)
            {
                return;
            }

            this.curAniType = aniType;
            for (int i = 0; i < this.aniList.Count; i++)
            {
                this.aniList[i].PlayAni(aniType);
            }
        }
    }

    internal class AniItem
    {
        private GameObject gameObject;
        private Vector3 initPos;
        private DirectionType directionType;
        /// <summary>
        /// 动画类型，0 收起 ，1 展开
        /// </summary>
        private int aniType = 1;
        public AniItem(GameObject gameObject,DirectionType directionType)
        {
            this.gameObject = gameObject;
            this.initPos = gameObject.transform.localPosition;
            this.directionType = directionType;
        }

        public void PlayAni(int aniType)
        {
            if (this.aniType == aniType)
            {
                return;
            }
            this.aniType = aniType;
            if(directionType == DirectionType.Up) this.PlayUp();
            if(directionType == DirectionType.Down)this.PlayDown();
            if(directionType == DirectionType.Left)this.PlayLeft();
            if(directionType == DirectionType.Right)this.PlayRight();
        }

        private void PlayUp()
        {
            if (this.aniType == 0)  //收起
            {
                this.gameObject.transform.DOLocalMoveY(this.initPos.y + 300,0.2f);
            }
            else if (this.aniType == 1) //展开
            {
                this.gameObject.transform.DOLocalMoveY(this.initPos.y,0.2f);
            }
        }

        private void PlayDown()
        {
            if (this.aniType == 0)
            {
                this.gameObject.transform.DOLocalMoveY(this.initPos.y- 300,0.2f);
            }
            else if (this.aniType == 1)
            {
                this.gameObject.transform.DOLocalMoveY(this.initPos.y ,0.2f);
            }
        }

        private void PlayLeft()
        {
            if (this.aniType == 0)
            {
                this.gameObject.transform.DOLocalMoveX(this.initPos.x- 700,0.4f);
            }
            else if (this.aniType == 1)
            {
                this.gameObject.transform.DOLocalMoveX(this.initPos.x ,0.2f);
            }
        }
        
        private void PlayRight()
        {
            if (this.aniType == 0)
            {
                this.gameObject.transform.DOLocalMoveX(this.initPos.x + 700,0.4f);
            }
            else if (this.aniType == 1)
            {
                this.gameObject.transform.DOLocalMoveX(this.initPos.x,0.2f);
            }
        }
    }
}