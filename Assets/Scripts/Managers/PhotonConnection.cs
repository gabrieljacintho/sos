using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Linq;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public static PhotonConnection instance;

    private RoomOptions roomOptions;
    [SerializeField]
    private byte maxPlayers = 4;
    private const int maxPlayerOnline = 20;

    private bool createRoom, findRoom, joinRoom;

    public delegate void MyDelegate();
    public MyDelegate myDelegate;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        Logar();
    }

    public void Logar()
    {
        PhotonNetwork.NickName = "Gabriel";
        StartCoroutine(Connect());
    }

    private IEnumerator Connect()
    {
        while (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            yield return new WaitForSeconds(5);
            if (!PhotonNetwork.IsConnected) Debug.Log("Sem conexão! Buscando...");
        }
    }

    public override void OnConnectedToMaster()
    {
        JoinLobby();
        Debug.Log("Conectado!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        StartCoroutine(Connect());
        // Ativar tela de aviso!
        Debug.Log("Conexão perdida!");
    }

    public void JoinLobby()
    {
        if (!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (createRoom) CreateRoom();
        else if (findRoom) FindRoom();
        else if (joinRoom) JoinRoom(UIManager.instance.GetCodeInputText());
        Debug.Log("Dentro do lobby!");
    }

    public void CreateRoom()
    {
        if (!PhotonNetwork.InLobby) createRoom = true;
        else if (!PhotonNetwork.InRoom)
        {
            createRoom = false;
            PhotonNetwork.CreateRoom(RandomAlfa(5), roomOptions);
        }
        UIManager.instance.ActivePanel("LoadingPanel");

    }

    public void FindRoom()
    {
        if (!PhotonNetwork.InLobby) findRoom = true;
        else if (!PhotonNetwork.InRoom)
        {
            findRoom = false;
            PhotonNetwork.JoinRandomOrCreateRoom(null, 4, MatchmakingMode.FillRoom,
                TypedLobby.Default, null, RandomAlfa(5), roomOptions);
        }
        UIManager.instance.ActivePanel("LoadingPanel");
    }

    public void JoinRoom(string code)
    {
        if (!PhotonNetwork.InLobby) joinRoom = true;
        else if (!PhotonNetwork.InRoom)
        {
            joinRoom = false;
            if (code.Length != 5) PhotonNetwork.JoinRandomRoom();
            else PhotonNetwork.JoinOrCreateRoom(code, roomOptions, TypedLobby.Default);
        }
        UIManager.instance.ActivePanel("LoadingPanel");
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            UIManager.instance.ActivePanel("LoadingPanel");
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        SpawnManager.instance.SpawnPlayer();
        UIManager.instance.SetRoomButtonText(PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Dentro da sala!");
    }

    public override void OnLeftRoom()
    {
        RoomManager.instance.SetMatchInProgress(false);
        RoomManager.instance.NextRound(true);
        UIManager.instance.ActivePanel("MenuPanel");
        Debug.Log("Fora da sala!");
    }

    public static string RandomAlfa(int tamanho)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        System.Random random = new System.Random();
        string result = new string(
            Enumerable.Repeat(chars, tamanho)
                      .Select(s => s[random.Next(s.Length)])
                      .ToArray());

        // Verificar se já existe uma sala com o mesmo nome

        return result;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(RandomAlfa(5), roomOptions);
        // Ativar tela de aviso!
        //Debug.Log("Houve um erro, tente novamente! Código: " + returnCode + "; Mensagem: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(RandomAlfa(5), roomOptions);
        // Ativar tela de aviso!
        //Debug.Log("Houve um erro, tente novamente! Código: " + returnCode + "; Mensagem: " + message);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(RandomAlfa(5), roomOptions);
        // Ativar tela de aviso!
        //if (returnCode == 32760) CreateRoom(); else
        //Debug.Log("Houve um erro, tente novamente! Código: " + returnCode + "; Mensagem: " + message);
    }
}
