using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Muzan.DTO;
using Muzan.Interfaces;
using Muzan.Models;
using Muzan.Services;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using YourProject.Configurations;

namespace Muzan.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        private readonly string _firebaseStorageUrl;
        public PostController(IUnitOfWork unitOfWork, HttpClient httpClient, IOptions<FirebaseSettings> firebaseSettings)
        {
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _firebaseStorageUrl = firebaseSettings.Value.StorageUrl;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var posts = _unitOfWork.Posts.GetAll();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var post = _unitOfWork.Posts.GetById(id);
            if (post == null)
            {

                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PostCreateDto postDto, IFormFile file)
        {
            if (postDto == null)
            {
                return BadRequest("Post object is null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var post = new Post
            {
                Title = postDto.Title,
                Text = postDto.Text,
            };

            if (file != null && file.Length > 0)
            {
                var fileUrl = await UploadFileToFirebaseStorage(file);
                post.ImageUrl = fileUrl;
            }

            _unitOfWork.Posts.Add(post);
            _unitOfWork.Save();

            return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            var existingPost = _unitOfWork.Posts.GetById(id);
            if (existingPost == null)
            {
                return NotFound();
            }

            _unitOfWork.Posts.Update(post);
            _unitOfWork.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var post = _unitOfWork.Posts.GetById(id);
            if (post == null)
            {
                return NotFound();
            }

            _unitOfWork.Posts.Remove(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private async Task<string> UploadFileToFirebaseStorage(IFormFile file)
        {
            var accessToken = await new FirebaseAuthService().GetAccessTokenAsync();

            var fileName = Path.GetFileName(file.FileName);
            var filePath = $"images/{Guid.NewGuid()}_{fileName}";

            using (var stream = file.OpenReadStream())
            {
                // Cria a solicitação HTTP para upload
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_firebaseStorageUrl}{Uri.EscapeDataString(filePath)}?uploadType=media");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = new StreamContent(stream);
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseBody);
                var downloadToken = jsonResponse["downloadTokens"].ToString(); // Ajuste conforme a resposta da API
                return $"{_firebaseStorageUrl}{Uri.EscapeDataString(filePath)}?alt=media&token={downloadToken}";
            }
        }

    }
    }
