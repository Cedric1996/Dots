using System;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using OpenMatch;

namespace UnityEngine.Ucg.Matchmaking
{
    public class Matchmaker
    {
        /// <summary>
        /// The hostname[:port]/{projectid} of your matchmaking server
        /// </summary>
        public string endpoint;
        readonly Frontend.FrontendClient m_client;
        private Channel channel;
        // MatchmakingController matchmakingController;
        private MatchmakingRequest request;
        private Assignment assignment;
        private Ticket ticket;
        // private OpenMatchClient m_client;
        public delegate void SuccessCallback(Assignment assignment);
        public delegate void ErrorCallback(string error);

        public SuccessCallback successCallback;
        public ErrorCallback errorCallback;

        public enum MatchmakingState
        {
            None,
            Requesting,
            Searching,
            Found,
            Error
        };

        /// <summary>
        /// The matchmaking state machine's current state
        /// </summary>
        public MatchmakingState State = MatchmakingState.None;

        /// <summary>
        /// Matchmaker
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="onSuccessCallback">If a match is found, this callback will provide the connection information</param>
        /// <param name="onErrorCallback">If matchmaking fails, this callback will provided some failure information</param>
        public Matchmaker(string endpoint, SuccessCallback onSuccessCallback = null, ErrorCallback onErrorCallback = null)
        {
            this.endpoint = endpoint;
            channel = new Channel(endpoint, ChannelCredentials.Insecure);
            m_client = new Frontend.FrontendClient(channel);
            this.successCallback = onSuccessCallback;
            this.errorCallback = onErrorCallback;
            ticket = TicketUtil.GenerateTicket(RandomString(6, true));
        }

        private void CreateTicket()
        {
            CreateTicketRequest request = new CreateTicketRequest();
            request.Ticket = ticket;
            State = MatchmakingState.Requesting;
            CreateTicketResponse ticketResponse = m_client.CreateTicket(request);
            ticket = ticketResponse.Ticket;
            if( ticket == null )
                return;             
        }

        private async Task GetAssignment(String Id)
        {
            GetAssignmentsRequest request = new GetAssignmentsRequest{
                TicketId = Id
            };
            try {
                State = MatchmakingState.Searching;
                var responseStream = m_client.GetAssignments(request).ResponseStream; 
                await responseStream.MoveNext();
                assignment = responseStream.Current.Assignment;
                Debug.Log(assignment);
                State = MatchmakingState.Found;
            } catch (RpcException e) {
                throw e;
            }
        } 

        /// <summary>
        /// Start Matchmaking
        /// </summary>
        /// <param name="playerId">The id of the player</param>
        /// <param name="playerProps">Custom player properties relevant to the matchmaking function</param>
        /// <param name="groupProps">Custom group properties relevant to the matchmaking function</param>
        // public async void RequestMatch(string playerId, MatchmakingPlayerProperties playerProps, MatchmakingGroupProperties groupProps)
        // {
        //     CreateTicketRequest();
        //     await GetAssignment(ticket.Id);
        //     //client.GetUpdates(m_ticket).Wait();

        //     await channel.ShutdownAsync();
        // }
        public async void RequestMatch(string playerId)
        {
            CreateTicket();
            await GetAssignment(ticket.Id);
            await channel.ShutdownAsync();
            Debug.Log(State);
        }
        
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            System.Random random = new System.Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        /// <summary>
        /// Matchmaking state-machine driver
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        // public void Update()
        // {
        //     switch (State)
        //     {
        //         case MatchmakingState.Requesting:
        //             matchmakingController.UpdateRequestMatch();
        //             break;
        //         case MatchmakingState.Searching:
        //             matchmakingController.UpdateGetAssignment();
        //             break;
        //         case MatchmakingState.Found:
        //         case MatchmakingState.None:
        //         case MatchmakingState.Error:
        //             break; // User hasn't stopped the state machine yet.
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        // }
    }
}
