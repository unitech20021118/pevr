using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Action.System
{
    public class ShootingModeHelp : MonoBehaviour
    {
        private bool _isShootingMode = false;
        float m_shootTimer = 0;
        private Camera _camera;
        private Transform _mfx;
        private AudioClip _audioClip;
        private Events shootingEvent; // 击中事件

        void Start()
        {
            _camera = gameObject.transform.GetComponentInParent<Camera>();
            _mfx = Resources.Load<Transform>("ModelPrefabs/FX");
            _audioClip = Resources.Load<AudioClip>("Sounds/shot");
        }

        void OnGUI()
        {
            if (_isShootingMode)
            {
                //绘制准星
                var texture = Resources.Load<Texture>("Images/hud");
                var texTurePos = new Rect(new Vector2(_camera.pixelWidth / 2 - (texture.width >> 1), _camera.pixelHeight / 2 - (texture.height >> 1)),
                    new Vector2(texture.width, texture.height));
                GUI.DrawTexture(texTurePos, texture);
            }
        }

        public void SetShootingMode(Events actionEvents)
        {
            _isShootingMode = true;
            shootingEvent = actionEvents;
        }

        void Update()
        {
            if (_isShootingMode)
            {
                OnFire();
            }
        }

        private void OnFire()
        {
            m_shootTimer -= Time.deltaTime;

            if (Input.GetMouseButton(0) && m_shootTimer <= 0)
            {
                m_shootTimer = 0.1f;
                //播放声音
                this.GetComponentInParent<AudioSource>().PlayOneShot(_audioClip);
                //准星位置
                var muzzlePoint = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
                var ray = _camera.ScreenPointToRay(muzzlePoint);
                RaycastHit info;
                var hit = Physics.Raycast(ray, out info, 100);
                if (hit)
                {
                    //产生击中特效
                    StartCoroutine(WaitforDestroy(info));
                    //如果对象物体没有FSM状态则返回 
                    if(info.transform.GetComponent<Main>()==null)
                        return;
                    //判断对象物体上是否有自定义的事件
                    var fsm = info.transform.GetComponent<Main>().GetFSM();
                    if (fsm.CurrentState().eventList.Find(a => a.name == shootingEvent.name) != null)
                    {
                        shootingEvent.DoRelateToEvents(); 
                    }
                }
            }
        }

        public IEnumerator WaitforDestroy(RaycastHit info)
        {
            //edit by kuai
            //var fx = Instantiate(_mfx, info.point, info.transform.rotation);
            //yield return new WaitForSeconds(1);
            //Destroy(fx.gameObject);
            GameObject obj = GetPool();
            obj.SetActive(true);
            obj.transform.position = info.point;
            obj.transform.rotation = info.transform.rotation;
            yield return new WaitForSeconds(1);
            AddPool(obj);
        }

        #region 射击模式的对象池

        private GameObject mfxGameObject;
        private Stack<GameObject> pool = new Stack<GameObject>();
        private int maxNum = 10;
        private GameObject CreateObj()
        {
            if (mfxGameObject==null)
            {
                mfxGameObject = Resources.Load<GameObject>("ModelPrefabs/FX");
            }
            return Instantiate(mfxGameObject);
        }
        private GameObject GetPool()
        {
            if (pool.Count>0)
            {
               return pool.Pop();
            }
            else
            {
                return CreateObj();
            }
        }

        private void AddPool(GameObject obj)
        {
            if (pool.Count<maxNum)
            {
                pool.Push(obj);
                obj.SetActive(false);
            }
            else
            {
                Destroy(obj);
            }
        }

        #endregion

    }
}
