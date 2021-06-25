﻿using System;
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
        //public string lastd = "";
        //public string lastd { get; set; }
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
            var body = new List<Point> {};
            var head = gameStatusRequest.You.Head;
            var rng = new Random();
            //////////nextPoint is init at head point
            var nextPoint = gameStatusRequest.You.Head;
            foreach (var bodypart in gameStatusRequest.You.Body)
            {
                body.Add(bodypart);
                Console.WriteLine($"body part : {bodypart.X},{bodypart.Y}");
            }
            Console.WriteLine($"head position : {head.X},{head.Y}");
            //Console.WriteLine($"throat position : {body[1].X},{body[1].Y}");
            //////////Nouvelle direction
            int newd = rng.Next(direction.Count);
            bool headinbody = true;
            while (headinbody==true)
            {   
                headinbody = false;
                Console.WriteLine("Head in body");
                nextPoint = gameStatusRequest.You.Head;
                newd = rng.Next(direction.Count);
                if (direction[newd]=="right")
                {
                    //nextPoint = new Point(nextPoint.X+1,nextPoint.Y);
                    nextPoint.X+=1;
                }
                else if (direction[newd]=="left")
                {
                    //nextPoint = new Point(nextPoint.X-1,nextPoint.Y);
                    nextPoint.X-=1;
                }
                else if (direction[newd]=="up")
                {
                    //nextPoint = new Point(nextPoint.X,nextPoint.Y-1);
                    nextPoint.Y+=1;
                }
                else if (direction[newd]=="down")
                {
                    //nextPoint = new Point(nextPoint.X,nextPoint.Y+1);
                    nextPoint.Y-=1;
                }
                foreach (var bodypart in body)
                {  
                    if (bodypart.X == nextPoint.X && bodypart.Y == nextPoint.Y)
                    {
                        headinbody=true;
                    }
                }
            }
            //Console.WriteLine($"{nextPoint.X},{nextPoint.Y}");
            //while (body.Contains(nextPoint))
            //{
                //Console.WriteLine("WARNING : HEAD IN THE BODY");
                //newd = rng.Next(direction.Count);
                // if forest  
            //}
            Console.WriteLine($"nextpoint : {nextPoint.X},{nextPoint.Y}");
            var response = new MoveResponse
            {
                Move = direction[newd],
                Shout = "I am moving!"
            };
            Console.WriteLine($"I move {direction[newd]}");
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