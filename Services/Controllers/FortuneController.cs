﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FortuneTeller.Services {
	[Route("")]
	public class FortuneController : Controller {
		private readonly IFortuneTeller _iFortuneTeller;
		private readonly ILogger<FortuneController> _logger;

		public FortuneController(IFortuneTeller iFortuneTeller, ILogger<FortuneController> logger) {
			_iFortuneTeller = iFortuneTeller;
			_logger = logger;
		}

		[HttpGet("fortunes")]
		public IEnumerable<Fortune> List() {
			return _iFortuneTeller.List();
		}

		[HttpGet("random")]
		public Fortune GetRandom() {
			_logger.LogInformation("A random fortune was requested");
			_logger.LogWarning("Warning: a random fortune was requested");
			_logger.LogError("Error: a random fortune was requested");

			return _iFortuneTeller.GetRandom();
		}

		[HttpGet("{id}")]
		public IActionResult Get(long id) {
			var fortune = _iFortuneTeller.Get(id);
			if ( fortune == null ) {
				return NotFound();
			}

			return new ObjectResult(fortune);
		}

		[HttpPost]
		public IActionResult Create([FromBody] Fortune fortune) {
			if ( fortune == null ) {
				return BadRequest();
			}

			_iFortuneTeller.Add(fortune);

			return CreatedAtRoute("Get", new { id = fortune.Id }, fortune);
		}

		[HttpPut("{id}")]
		public IActionResult Update(long id, [FromBody] Fortune fortune) {
			if ( fortune == null || fortune.Id != id ) {
				return BadRequest();
			}

			var f = _iFortuneTeller.Get(id);
			if ( f == null ) {
				return NotFound();
			}

			fortune.Text = f.Text;

			_iFortuneTeller.Update(fortune);
			return new NoContentResult();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id) {
			var fortune = _iFortuneTeller.Get(id);
			if ( fortune == null ) {
				return NotFound();
			}

			_iFortuneTeller.Remove(id);
			return new NoContentResult();
		}
	}
}