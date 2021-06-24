using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Starter.Api.Requests;
using Starter.Api.Responses;
using Starter.Core;

namespace Starter.Api.Controllers
{
    [ApiController]
    public class SnakeController : ControllerBase
    {   
        public string lastd = "";
        /// <summary>
        /// This request will be made periodically to retrieve information about your Battlesnake,
        /// including its display options, author, etc.
        /// </summary>
        [HttpGet("")]
        public IActionResult Index()
        {
            var response = new InitResponse
            {   
                ApiVersion = "1",
                Author = "",
                Color = "#800000",
                Head = "beluga",
                Tail = "curled"
            };

            return Ok(response);
        }


        /// <summary>
        /// Your Battlesnake will receive this request when it has been entered into a new game.
        /// Every game has a unique ID that can be used to allocate resources or data you may need.
        /// Your response to this request will be ignored.
        /// </summary>
        [HttpPost("start")]
        public IActionResult Start(GameStatusRequest gameStatusRequest)
        {
            return Ok();
        }


        /// <summary>
        /// This request will be sent for every turn of the game.
        /// Use the information provided to determine how your
        /// Battlesnake will move on that turn, either up, down, left, or right.
        /// </summary>
        [HttpPost("move")]
        public IActionResult Move(GameStatusRequest gameStatusRequest)
        {
            var direction = new List<string> {"down", "left", "right", "up"};
            var oposite = new List<string> {"up", "right", "left", "down"};
            var rng = new Random();
            int newd = rng.Next(direction.Count);
            if (lastd != "")
            {
                while (oposite[direction.IndexOf(lastd)] != direction[newd])
                {
                    newd = rng.Next(direction.Count);
                }
            }
            
            var response = new MoveResponse
            {
                Move = direction[newd],
                Shout = "I am moving!"
            };
            lastd = response.Move;
            foreach (var bodypart in gameStatusRequest.You.Body)
            {
                Console.WriteLine(bodypart.X);
                Console.WriteLine(bodypart.Y);
            }
            Console.WriteLine(lastd);
            return Ok(response);
        }


        /// <summary>
        /// Your Battlesnake will receive this request whenever a game it was playing has ended.
        /// Use it to learn how your Battlesnake won or lost and deallocated any server-side resources.
        /// Your response to this request will be ignored.
        /// </summary>
        [HttpPost("end")]
        public IActionResult End(GameStatusRequest gameStatusRequest)
        {
            return Ok();
        }
    }
}