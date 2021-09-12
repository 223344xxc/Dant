using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum AttackType
{
    None,
    NormalAttack,
    TopAttack,
    TopSkill,
    BottomAttack,
    JumpTopAttack,
    JumpBottonAttack,
    SkillGrap,
    Ultimate,
    AirDownSkill,
}

public class PlayerCtrl : PlayerAbility
{
    private static PlayerCtrl instance;
    public static PlayerCtrl Instance => instance;
    public Vector3 playerCenterOffset;

    private float combo = 0;
    public float Combo
    {
        get => combo;
        set
        {
            combo = value;
            CancelInvoke("ComboReset");
            UpdatePlayerUI?.Invoke(this);
            Invoke("ComboReset", ComboResetTime);
        }
    }
    public float ComboResetTime;
    public HpBarCtrl hpBar;

    [SerializeField] private GameObject TopAttackEffect;
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private Transform projectilePoint;
    private Dictionary<AttackType, GameObject> AttackColliders;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer renderer;
    private Color TempColor;

    private bool IsJump = false;
    private bool IsAttack = false;
    private bool IsDash = false;

    private float DashDistance = 0;
    private bool PassibleMove = true;
    private bool IsJumpTopAttack = false;

    private Vector2 moveVector;
    public Vector2 MoveVector
    {
        get
        {
            if (IsDash)
            {
                moveVector.Set(DashDistance * MoveSpeed * DashSpeed, rigid.velocity.y);
                return moveVector;
            }

            if (PassibleMove)
                moveVector.Set(IsJump ? rigid.velocity.x : JoyStickCtrl.JoyStickPosition.x * MoveSpeed, rigid.velocity.y);
            else
                moveVector.Set(IsJump ? rigid.velocity.x : 0, rigid.velocity.y);
            if (Mathf.Abs(JoyStickCtrl.JoyStickPosition.x) > 3)
                moveScale.Set(JoyStickCtrl.JoyStickPosition.x > 0 ? -1 * StartScale.x : 1 * StartScale.x, StartScale.y, StartScale.z);

            return moveVector;
        }
    }

    private Vector3 StartScale;
    private Vector3 moveScale;
    public Vector3 MoveScale
    {
        get => moveScale;
    }

    private Vector2 JumpVelocity;
    public Vector2 JumpVector
    {
        get
        {
            JumpVelocity.Set(0, JumpPower);
            return JumpVelocity;
        }
    }

    private Vector3 DumyVector;

    public delegate void AttackObject(int damage, float impulse, Action eventAction);

    private static AttackObject AttackToObject;

    private static Action<PlayerCtrl> updatePlayerUI;
    public static Action<PlayerCtrl> UpdatePlayerUI
    {
        get => updatePlayerUI;
        set => updatePlayerUI = value;
    }


    private static Action gameOver;

    public AttackType NowAttackType;

    [Header("착지 판정 원")]
    public Vector3 CirclePos;
    public float Radius;

    public override void Awake()
    {
        InitPlayer();
    }
    public void Update()
    {
        PlayerMove();
        ChackGround();
    }

    public void OnDestroy()
    {
        if(instance != null)
        {
            instance = null;
        }
    }

    private void FixedUpdate()
    {

        PlayerRigidbodyUpdate();
    }

    private void InitPlayer()
    {
        if (instance is null)
        {
            instance = this;
        }
        base.Awake();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        StartScale = transform.localScale;
        moveScale = StartScale;
        InitAttackColliders();
    }
    private void InitAttackColliders()
    {
        AttackColliders = new Dictionary<AttackType, GameObject>();


        AddAttackCollider(AttackType.NormalAttack, "AttackArea");
        AddAttackCollider(AttackType.TopAttack, "TopAttackArea");
        AddAttackCollider(AttackType.JumpTopAttack, "JumpTopAttackArea");
        AddAttackCollider(AttackType.JumpBottonAttack, "JumpBottomAttackArea");
        AddAttackCollider(AttackType.BottomAttack, "BottomAttackArea");
        AddAttackCollider(AttackType.SkillGrap, "SkillGrapAttackArea");
        AddAttackCollider(AttackType.Ultimate, "UltimateAttackArea");
        AddAttackCollider(AttackType.AirDownSkill, "AirDownSkillAttackArea");
        AddAttackCollider(AttackType.TopSkill, "TopSkillAttackArea");
    }
    private void AddAttackCollider(AttackType Key, string ColliderName)
    {
        AttackColliders.Add(Key, transform.Find(ColliderName).gameObject);
    }
    private void PlayerRigidbodyUpdate()
    {
        if (isDeath)
            return;
        rigid.velocity = MoveVector;
    }
    private void PlayerMove()
    {
        if (isDeath)
            return;
        if (!IsAttack)
            transform.localScale = MoveScale;

        if (Input.GetKeyDown(KeyCode.Space) && !IsJump)
        {
            Jump();
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Attack();
        //}
        //if(Input.GetMouseButtonDown(1))
        //{
        //    Skill();
        //}
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ultimate();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Dash();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(Invincibility());
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Damaged(1);
        }

        if (JoyStickCtrl.StickFollow && Mathf.Abs(JoyStickCtrl.JoyStickPosition.x) > 3)
        {
            anim.SetBool("IsMove", true);
        }
        else
        {
            anim.SetBool("IsMove", false);
        }


        if (transform.position.y <= -20)
        {
            transform.position = new Vector3(0, 5, 0);
            Damaged(1);
        }
    }
    private void ChackGround()
    {
        if (rigid.velocity.y < 0/* || (rigid.velocity.y <= 0 && IsJump == true)*/)
        {
            RaycastHit2D[] groundhit = Physics2D.CircleCastAll(CirclePos + transform.position, Radius, Vector2.zero);
            for (int i = 0; i < groundhit.Length; i++)
            {
                if (groundhit[i].transform.CompareTag("GROUND"))
                {
                    if (IsJump && isDeath)
                        NormalDeath();


                    if (NowAttackType == AttackType.AirDownSkill)
                        AirDownRending();
                    IsJumpTopAttack = false;
                    IsJump = false;


                    break;
                }
                if (groundhit.Length - 1 == i)
                {
                    IsJump = true;
                }
            }
        }
    }
    public void ComboReset()
    {
        if (Combo > 0)
            Combo = 0;
    }
    public override void Damaged(int Damage)
    {
        if (isInvincibility)
            return;
        base.Damaged(Damage);
        hpBar.HpBarDamage(Hp);
        if (!isDeath)
        {
            StartCoroutine(Invincibility());
        }
        if (isDeath)
            Invoke("GameOver", 3);
    }
    private IEnumerator Invincibility()
    {
        isInvincibility = true;
        WaitForSeconds HideTime = new WaitForSeconds((InvincibilityTime * 0.5f) / (HideCount_Start * 2));

        for(int i = 0; i < 3; i++)
        {
            SetPlayerAlpha(0.5f);
            yield return HideTime;

            SetPlayerAlpha(1f);
            yield return HideTime;
        }

        HideTime = new WaitForSeconds((InvincibilityTime * 0.5f) / (HideCount_End * 2));

        for (int i = 0; i < 5; i++)
        {
            SetPlayerAlpha(0.5f);
            yield return HideTime;

            SetPlayerAlpha(1f);
            yield return HideTime;
        }

        SetPlayerAlpha(1);
        isInvincibility = false;
    }

    private void GameOver()
    {
        gameOver?.Invoke();
    }
    private void SetPlayerAlpha(float value)
    {
        TempColor = renderer.color;
        TempColor.a = value;
        renderer.color = TempColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(CirclePos + transform.position, Radius);
    }

    #region 액션 추가 삭제 함수
    public static void AddAttackToObject(AttackObject ObjectFun)
    {
        AttackToObject += ObjectFun;
    }
    public static void RemoveAttackToObject(AttackObject ObjectFun)
    {
        AttackToObject -= ObjectFun;
    }
    public static void AddGameOverFun(Action over)
    {
        gameOver = over;
    }
    #endregion

    #region 버튼 함수들
    public void Dash()
    {
        if (!IsDash && PassibleMove)
            anim.Play("Dash");
    }

    public void Jump()
    {
        if (IsJump)
            return;

        rigid.AddForce(JumpVector);
        IsJump = true;
    }

    public void Attack()
    {
        if (IsAttack || IsDash)
            return;

        if (JoyStickCtrl.StickDirection == JoyStickDirection.UP && IsJump == false)
            TopAttack();
        else if (JoyStickCtrl.StickDirection == JoyStickDirection.UP && IsJump == true)
            JumpTopAttack();
        else if (JoyStickCtrl.StickDirection == JoyStickDirection.DOWN && IsJump == true)
            JumpBottomAttack();
        else if (JoyStickCtrl.StickDirection == JoyStickDirection.DOWN && IsJump == false)
            DownAttack();
        else
            NormalAttack();
    }

    public void Skill()
    {
        if (IsAttack || IsDash)
            return;

        if (JoyStickCtrl.StickDirection == JoyStickDirection.DOWN && IsJump == false)
            SkillGrap();
        else if (JoyStickCtrl.StickDirection == JoyStickDirection.UP && IsJump == false)
            TopSkill();
        else if (IsJump == true)
            AirDownSkill();
        else
            NormalSkill();
    }

    public override void Death()
    {
        base.Death();

        if (IsJump)
            AirDeath();
        else
            NormalDeath();
    }
    #endregion

    #region 코드에서 불리는 함수들
    public void Ultimate()
    {
        if (IsAttack || IsDash)
            return;

        NowAttackType = AttackType.Ultimate;
        anim.Play("Ultimate");
    }
    private void NormalAttack()
    {
        NowAttackType = AttackType.NormalAttack;
        anim.Play("Attack_1");
    }
    private void NormalSkill()
    {
        anim.Play("Skill");
    }
    private void TopSkill()
    {
        NowAttackType = AttackType.TopSkill;
        anim.Play("TopSkill");
    }
    private void AirDownSkill()
    {
        NowAttackType = AttackType.AirDownSkill;
        anim.Play("AirDownFlySkill");
    }
    private void TopAttack()
    {
        GameObject Effect = Instantiate(TopAttackEffect);
        NowAttackType = AttackType.TopAttack;
        Effect.transform.position = transform.position;
        DumyVector.Set(transform.localScale.x < 0 ? -1 * Effect.transform.localScale.x : 1 * Effect.transform.localScale.x, Effect.transform.localScale.y, Effect.transform.localScale.z);
        Effect.transform.localScale = DumyVector;
        IsJump = true;
        anim.Play("TopAttack");
        rigid.AddForce(Vector2.up * 2000);
    }
    private void DownAttack()
    {
        NowAttackType = AttackType.BottomAttack;
        anim.Play("DownAttack");
    }
    private void JumpBottomAttack()
    {
        NowAttackType = AttackType.JumpBottonAttack;
        anim.Play("JumpBottomAttack");
    }
    private void JumpTopAttack()
    {
        if (IsJumpTopAttack)
            return;
        NowAttackType = AttackType.JumpTopAttack;
        IsJumpTopAttack = true;
        rigid.AddForce(Vector2.up * 2000);
        anim.Play("JumpTopAttack");

    }
    private void SkillGrap()
    {
        NowAttackType = AttackType.SkillGrap;
        anim.Play("Attack_Grap");
    }
    private void AirDeath()
    {
        isDeath = true;
        anim.Play("Air_Death");
    }
    private void NormalDeath()
    {
        isDeath = true;
        anim.Play("NormalDeath");
    }
    private void AirDownRending()
    {
        //NowAttackType = AttackType.None;
        anim.Play("AirDownRending");
    }
    #endregion

    #region 이밴트 함수들
    public void SpawnProjectile()
    {
        GameObject bullet = Instantiate(projectilePrefab);
        bullet.transform.position = projectilePoint.position;
        bullet.transform.localScale = transform.localScale;
    }

    public void ForceJump(float Power)
    {
        IsJump = true;
        rigid.AddForce(Vector2.up * Power);
    }
    public void StartDash()
    {
        DashDistance = transform.localScale.x > 0 ? -1 : 1;
        PassibleMove = false;
        IsDash = true;
    }

    public void EndDash()
    {
        DashDistance = 0;
        PassibleMove = true;
        IsDash = false;
    }

    public void SetAttack()
    {
        IsAttack = true;
    }

    public void AttackRelease()
    {
        IsAttack = false;
    }

    public void MoveStop()
    {
        PassibleMove = false;
    }

    public void MoveRelease()
    {
        PassibleMove = true;
    }

    public void ActiveAttackArea()
    {
        if (AttackColliders.ContainsKey(NowAttackType))
        {
            AttackColliders[NowAttackType].SetActive(!AttackColliders[NowAttackType].activeInHierarchy);
        }
    }

    public void ResetAttackType()
    {
        NowAttackType = AttackType.None;
    }

    public void AttackToHitObject()
    {
        if (NowAttackType == AttackType.SkillGrap)
        {
            AttackToObject?.Invoke(AttackDamage, -2, () => Combo += 1);
        }
        else
            AttackToObject?.Invoke(AttackDamage, 0.5f, () => Combo += 1);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(transform.position + playerCenterOffset, 0.1f);
    }
}
