using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StormyLib.DbInteraction;
using StormyLib.Models;

namespace StormyWebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PostController(IDbInteraction<Post> dbInteraction) : ControllerBase
{
    private readonly IDbInteraction<Post> _dbInteraction = dbInteraction;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var post = await _dbInteraction.GetAsync(id);
        if (post == null)
        {
            return NoContent();
        }
        return Ok(post);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Post post)
    {
        var result = await _dbInteraction.AddAsync(post);
        if (result == null)
        {
            return BadRequest();
        }

        return CreatedAtRoute("Post", new { id = result.PostId }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Put(Post post)
    {
        var result = await _dbInteraction.UpdateAsync(post);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _dbInteraction.DeleteAsync(id);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result);
    }
}
