using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StormyLib.DbInteraction;
using StormyLib.Models;

namespace StormyWebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class BlogController(IDbInteraction<Blog> blogDbInteraction) : ControllerBase
{
    private readonly IDbInteraction<Blog> _dbInteraction = blogDbInteraction;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var blog = await _dbInteraction.GetAsync(id);
        if (blog == null)
        {
            return NoContent();
        }
        return Ok(blog);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Blog blog)
    {
        var result = await _dbInteraction.AddAsync(blog);
        if (result == null)
        {
            return BadRequest();
        }

        return CreatedAtRoute("Blog", new { id = result.BlogId }, result);
    }

    [HttpPost("addPost/{id}")]
    public async Task<IActionResult> AddPost(int id, Post post)
    {
        var blog = await _dbInteraction.GetAsync(id);
        if (blog == null)
        {
            return Content("Blog not found");
        }
        blog.Posts.Add(post);
        var result = await _dbInteraction.UpdateAsync(blog);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Put(Blog blog)
    {
        var result = await _dbInteraction.UpdateAsync(blog);
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
