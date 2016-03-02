using Hexocracy.HelpTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hexocracy.Core
{
    public class Cannon : Building
    {
        public override ContentType Type { get { return ContentType.Building; } }

        public override BuildingType BType { get { return BuildingType.Cannon; } }

        public GameObject projectilePrototype;

        private Transform turret;
        private Transform shotPoint;

        protected override void Awake()
        {
            base.Awake();
            turret = t.Find("Turret");
            shotPoint = turret.Find("Barrel").Find("ShotPoint");
        }

        private Quaternion destinationRotation;
        private Quaternion originRotation;

        //private Vector3 destEA;
        //private Vector3 origingEA;

        public float speed = 20;
        private float time;
        private float currTime;
        //public bool rotating;

        private bool canAim = true;
        private bool canShot = false;
        private bool canRotate = false;
        private Hex target;

        private void RotateToTarget(Vector3 target)
        {
            var currPosition = turret.position;

            var r1 = shotPoint.position - currPosition;
            r1.Normalize();
            //r1.y = 0;

            //var a1 = Mathf.Rad2Deg * Mathf.Atan(r1.z / r1.x);

            var r2 = target - currPosition;

            r2.Normalize();
            r2.y = 0.833f;
            //var a2 = Mathf.Rad2Deg * Mathf.Atan(r2.z / r2.x);

            //Vector3.Dot(r1, r2);

            //Debug.Log(currPosition + " " + target);
            originRotation = Quaternion.FromToRotation(Vector3.up, r1);
            destinationRotation = Quaternion.FromToRotation(Vector3.up, r2);

            //origingEA = turret.rotation.eulerAngles;
            //destEA = destinationRotation.eulerAngles;

            //angle = Mathf.DeltaAngle(a1, a2);
            var angle = Quaternion.Angle(originRotation, destinationRotation);

            time = angle / speed;

            currTime = time;
            //Debug.Log(destinationRotation);
            //Debug.Log(angle);
            //Debug.Log(shotPoint.position.y);

            //turret.rotation = destinationRotation;
            //rotating = true;
        }

        private void Update()
        {
            if (canAim && Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, 1 << 8))
                {
                    target = hit.collider.GetComponent<Hex>();

                    var v = HexInfo.IndexToVector(target.Index);
                    RotateToTarget(v);
                    canAim = false;
                    canRotate = true;
                }
            }

            if (canRotate)
            {
                currTime -= Time.deltaTime;
                turret.rotation = Quaternion.Slerp(originRotation, destinationRotation, 1 - (currTime / time));
                if (currTime <= 0)
                {
                    canRotate = false;
                    canShot = true;
                }
            }

            if (canShot)
            {
                canShot = false;
                Shot();
            }
        }



        private void Shot()
        {
            var projectile = (GameObject)Instantiate(projectilePrototype, shotPoint.position, Quaternion.identity);
            float t;
            var v = Kinematics.CalculateVelocity(shotPoint.position, target.GroundCenter, Mathf.Deg2Rad * 65, out t);
            projectile.GetComponent<Rigidbody>().AddForce(v, ForceMode.Impulse);
            projectile.GetComponent<Rigidbody>().mass = 0.01f;

            StartCoroutine(OnShot(t, projectile));
        }

        private IEnumerator OnShot(float t, GameObject projectile)
        {
            yield return new WaitForSeconds(t);

            GameObject.Destroy(projectile);
            if (target.Content.Type == ContentType.Figure)
            {
                ((Figure)target.Content).OnAttack(null, 25);
            }

            canAim = true;
        }
    }
}
