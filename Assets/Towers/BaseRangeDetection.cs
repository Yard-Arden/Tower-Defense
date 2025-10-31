using System.Collections.Generic;
using UnityEngine;

public class BaseRangeDetection : MonoBehaviour
{
   #region Vairableas
   [Header("Detection Settings")]
   [SerializeField] private string targetTag = "Enemy";
   [SerializeField] private bool useDebug = true;

   [Header("Mode")]
   [SerializeField] private DetectionMode currentDetectMode = DetectionMode.Vector;
   private enum DetectionMode { None, Trigger, Vector }

   [Header("Vector Detection")]
   [SerializeField, Range(0f, 35f)] private float detectionRange = 10f;
   [SerializeField] private float checkInterval = 0.25f;
   private float timer;
   private readonly HashSet<RegisterAsEnemy> inRange = new HashSet<RegisterAsEnemy>();

   #endregion


   #region Update
   void Update()
   {
      if (currentDetectMode == DetectionMode.Vector)
      {
         timer += Time.deltaTime;
         if (timer >= checkInterval)
         {
            timer = 0f;
            DetectNearest_Vector();
         }
      }
      else if (currentDetectMode == DetectionMode.Trigger)
      {
         timer += Time.deltaTime;
         if (timer >= checkInterval)
         {
            timer = 0f;
            DetectNearest_TriggerSet();
         }
      }
   }

   #endregion

   #region Detect-Verctor
   private void DetectNearest_Vector()
   {
      RegisterAsEnemy nearest = null;
      float best = float.PositiveInfinity;
      Vector2 me = transform.position;
      float r2 = detectionRange * detectionRange;

      foreach (var e in RegisterAsEnemy.allEnemys)
      {
         if (!e || !e.gameObject.activeInHierarchy) continue;
         if (!string.IsNullOrEmpty(targetTag) && !e.CompareTag(targetTag)) continue;

         float d2 = ((Vector2)e.transform.position - me).sqrMagnitude;
         if (d2 <= r2 && d2 < best)
         {
            best = d2;
            nearest = e;
         }
      }

      if (nearest && useDebug) Debug.DrawLine(transform.position, nearest.transform.position, Color.red);

      // TODO: Shootlogic
   }

   #endregion


   #region Detect-Trigger
   private void DetectNearest_TriggerSet()
   {
      RegisterAsEnemy nearest = null;
      float best = float.PositiveInfinity;
      Vector2 me = transform.position;

      foreach (var e in inRange)
      {
         if (!e || !e.gameObject.activeInHierarchy) continue;
         if (!string.IsNullOrEmpty(targetTag) && !e.CompareTag(targetTag)) continue;

         float d2 = ((Vector2)e.transform.position - me).sqrMagnitude;
         if (d2 < best)
         {
            best = d2;
            nearest = e;
         }
      }

      if (nearest && useDebug) Debug.DrawLine(transform.position, nearest.transform.position, Color.red);

      // TODO: Shootlogic
   }

   #endregion

   #region Trigger-Settings
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (currentDetectMode != DetectionMode.Trigger) return;
      if (!other.CompareTag(targetTag)) return;

      var enemy = other.GetComponentInParent<RegisterAsEnemy>();
      if (enemy)
      {
         inRange.Add(enemy);
      }

      DetectNearest_TriggerSet();
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (currentDetectMode != DetectionMode.Trigger) return;
      if (!other.CompareTag(targetTag)) return;

      var enemy = other.GetComponentInParent<RegisterAsEnemy>(); //Regisfter
      if (enemy)
      {
         inRange.Remove(enemy);
      }
   }

   #endregion


   #region On-Validate

   private void OnValidate() //Chenges to Inpektor use for Enable Colliders
   {
      if (currentDetectMode == DetectionMode.Trigger)
      {
         var col = GetComponent<Collider2D>();
         if (col && !col.isTrigger) col.isTrigger = true;
      }
   }

   #endregion


   #region Gizmos

   private void OnDrawGizmos()
   {
      if (!useDebug) return;

      Gizmos.color = new Color(0.4f, 0.6f, 1f, 0.7f);
      if (currentDetectMode == DetectionMode.Vector)
      {
         Gizmos.DrawWireSphere(transform.position, detectionRange);
      }
   }

   #endregion
}
