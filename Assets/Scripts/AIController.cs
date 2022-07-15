using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    public static AIController ai;
    public Text kill;

    NavMeshAgent navMeshAgent;               //  Nav mesh agent component
    public float startWaitTime = 4;                 //  Wait time of every action
    public float timeToRotate = 2;                  //  Wait time when the enemy detect near the player without seeing
    public float speedWalk = 3;                     //  Walking speed, speed in the nav mesh agent
    public float speedRun = 3;                      //  Running speed

    public float viewRadius = 15;                   //  Radius of the enemy view
    public float viewAngle = 90;                    //  Angle of the enemy view
    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;                  //  To detect the obstacules with the raycast
    public float meshResolution = 1.0f;             //  How many rays will cast per degree
    public int edgeIterations = 4;                  //  Number of iterations to get a better performance of the mesh filter when the raycast hit an obstacule
    public float edgeDistance = 0.5f;               //  Max distance to calcule the a minumun and a maximum raycast when hits something
    public ParticleSystem muzzleFlash;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    public Material fovRedMaterial;
    public Material fovWhiteMaterial;
    public ParticleSystem cloud;

    public Transform[] waypoints;                   //  All the waypoints where the enemy patrols
    int m_CurrentWaypointIndex;                     //  Current waypoint where the enemy is going to
    public Vector3[] rotations;
    int m_CurrentRotationpoint;  

    public Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    public Vector3 m_PlayerPosition;                       //  Last position of the player when the player is seen by the enemy

    public float m_WaitTime;                               //  Variable of the wait time that makes the delay
    public float m_TimeToRotate;                           //  Variable of the wait time to rotate when the player is near that makes the delay
    public bool m_playerInRange;                           //  If the player is in range of vision, state of chasing
    public bool m_PlayerNear;                              //  If the player is near, state of hearing
    public bool m_IsPatrol;                                //  If the enemy is patrol, state of patroling
    public bool m_CaughtPlayer;                            //  if the enemy has caught the player
    public TriggerControl trcontrol;
    Animator animator;
    public bool kovala=false;
    public float mesafe;
    public Transform player;
    public int attackDamage = 25;
    public float attackRate = 0.5f;
    float nextAttackTime=0f;
    public Player player1;
    public bool isDead = false;
    public bool isMoveable =true;
     public int numberOfEnemies;
     public Vector3 enemyfirstlocation;
     public Vector3 enemyfirstrotation;
     public float delay=1f;
     bool timer=true;
     bool isRotate=true;
     bool Isarrived=true;
    private void Awake()
    {
        ai = this;
    }
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;                 //  Set the wait time variable that will change
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;                 //  Set the initial waypoint
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;             //  Set the navemesh speed with the normal speed of the enemy
        if (isMoveable)
        {
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the destination to the first waypoint
        }
        else
        {
            enemyfirstlocation=transform.position;
            enemyfirstrotation=transform.eulerAngles;
            
        }
        
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        PlayerPrefs.SetInt("Enemies", GameObject.FindGameObjectsWithTag("Enemy").Length);
    }

    private void Update()
    {
           mesafe = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
            EnviromentView();                       //  Check whether or not the player is in the enemy's field of vision

       
        if (!m_IsPatrol&&!trcontrol.safe)
        {
            Chasing();
        }
        else
        {
            m_IsPatrol=true;
            if (isMoveable)
            {
                 Patroling();
            }
            else
            {
                
                if (!Isarrived)
                {
                    Move(speedWalk);
                    navMeshAgent.SetDestination(enemyfirstlocation);
                    Isarrived=true;
                }
                
                

                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance&&!isRotate)
                {
                    transform.eulerAngles=enemyfirstrotation;
                    Stop();
                    isRotate=true;
                }   
                
                transform.Rotate(rotations[m_CurrentRotationpoint]*Time.deltaTime);
                
                if (m_WaitTime <= 0)
                {
                    NextRotation();
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    m_WaitTime -= Time.deltaTime;
                }

            }
           
        }
        if(m_playerInRange)
        {
            viewMeshFilter.GetComponent<MeshRenderer>().material = fovRedMaterial;
        }
        else
        {
            viewMeshFilter.GetComponent<MeshRenderer>().material = fovWhiteMaterial;
        }
        if (!player.GetComponent<BoxCollider>().Equals(null))
        {
            if (!isDead)
            {
                kill.gameObject.SetActive(true);
                kill.gameObject.transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            kill.gameObject.SetActive(false);
        }
    }
    void LateUpdate()
    {
        DrawFieldOfView();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("TrapObject") && !isDead)
        {
            cloud.Play();
            StartCoroutine(TakeDamage());
            collision.gameObject.tag = "Obstacle";
            collision.gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }
    }

    public void Chasing()
    {   
        //  The enemy is chasing the player
        Isarrived=false;
        isRotate=false;
        m_PlayerNear = false;                       //  Set false that hte player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position

         if (!m_CaughtPlayer)

        {
            
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);          //  set the destination of the enemy to the player location
        }

       
        if (!kovala)    //  Control if the enemy arrive to the player location
        {
                if (!isMoveable)
                {
                    m_IsPatrol = true;
                    
                    
                }
           
            if (m_WaitTime <= 0 && !m_CaughtPlayer )
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);

            }
            else
            {
                Debug.Log("Stop00");
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Debug.Log("Stop11");
                    //  Wait if the current position is not the player position
                    animator.SetBool("IsWalk",false);
                    Stop();
                    }
                m_WaitTime -= Time.deltaTime;
                
            }
        }
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= 4f)
        {
            Stop();
        }


    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            //  Check if the enemy detect near the player, so the enemy will move to that position
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                //  The enemy wait for a moment and then go to the last player position
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            Move(speedWalk);
            m_PlayerNear = false;           //  The player is no near when the enemy is platroling
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    //  Set the enemy destination to the next waypoint
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                //  If the enemy arrives to the waypoint position then wait for a moment and go to the next
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAnimatorMove()
    {

    }
    private void Attack()
    {
        if(!Player.instance.isDie && !isDead)
        {
            animator.SetTrigger("Attack");
            player.GetComponent<Player>().TakeDamage(attackDamage);
            muzzleFlash.Play();
        }
    }
    private void WaitBeforeAttack()
    { 
        if (timer)
        {
            nextAttackTime=Time.time+delay;
            timer=false;
        }
        
        if (Vector3.Distance(transform.position, player.position) > viewRadius)
        {
            /*
                *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                *  Or the enemy is a safe zone, the enemy will no chase
                * */
            m_playerInRange = false;                //  Change the sate of chasing
            
            
        }
              
        if (m_playerInRange)
            {
                
                if(Time.time >= nextAttackTime && player1.currentHealth > 0 )
                {
                    Stop();
                    Attack();
                }

            }
            else
            {
                timer=true;
            }
        
    }

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }
        public void NextRotation()
    {
         m_CurrentRotationpoint = (m_CurrentRotationpoint + 1) % rotations.Length;
    }
    


    void Stop()
    {
        animator.SetBool("IsWalk",false);
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    void Move(float speed)
    {
        animator.SetBool("IsWalk",true);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    public void EnviromentView()
    {
        
        if (trcontrol.safe)
        { 
            kovala=false;
            m_CaughtPlayer=false;
            m_playerInRange=false;
            m_PlayerNear = false;
            
        }
         Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);   //  Make an overlap sphere around the enemy to detect the playermask in the view radius

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            { 
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask)&&!(trcontrol.safe))
                {
                    kovala=true;
                    m_playerInRange = true;             //  The player has been seeing by the enemy and then the enemy starts to chasing the player
                    m_IsPatrol = false;                 //  Change the state to chasing the player
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    
                    m_playerInRange = false;
                }
            }
            else
            {
                 m_playerInRange = false;
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius &&!kovala)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                m_playerInRange = false;                //  Change the sate of chasing
            }
            WaitBeforeAttack();
            

        }
            if (kovala)
            {
                
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision

            }
    }
    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDistance;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDistance;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
    public IEnumerator TakeDamage()
    {
        PlayerPrefs.SetInt("Enemies", PlayerPrefs.GetInt("Enemies")-1);
        numberOfEnemies= PlayerPrefs.GetInt("Enemies");
        animator.SetTrigger("Death");
        isDead = true;
        transform.Find("ViewVisualisation").gameObject.SetActive(false);
        this.gameObject.GetComponent<AIController>().enabled = false;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        this.gameObject.transform.GetChild(4).gameObject.SetActive(false);
        this.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        Stop();
        AgentCount.instance.agentCount--;
        AgentCount.instance.agentCountText.text = "" + AgentCount.instance.agentCount;
        yield return new WaitForSeconds(1f);
        if (gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled == true)
        {
            kill.gameObject.SetActive(false);
        }
        
    }


}