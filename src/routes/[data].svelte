<script>
	import { page } from '$app/stores';
	import { firstLetterToUpper } from './../lib/lib.js';

	const data = $page.params.data;
	const dataCleaned = firstLetterToUpper(data);

	let hue = 50;
	let fontHue = 0;

	setInterval(() => {
		hue++;
		fontHue = hue + 250;
	}, 50);

	const saying = `Go\nFuck\nYourself!`;
</script>

<svelte:head>
	<title>Yeet</title>
	<meta name="description" content="Cause this person loves you!" />
</svelte:head>

<div
	style="color:hsl({fontHue}, 50%, 50%);background-color:hsl({hue}, 50%, 50%)"
	class="saying-container"
>
	<div class="saying">{dataCleaned},</div>
	<div class="saying" style="text-decoration: underline;">{saying}</div>
</div>

<style>
	.saying-container {
		position: absolute;

		top: 0;
		right: 0;
		bottom: 0;
		left: 0;

		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
	}

	@font-face {
		font-family: 'Pacifico';
		src: url('../resources/Pacifico-Regular.ttf');
	}

	.saying {
		font: 400 100px/1.3 'Pacifico', Helvetica, sans-serif;
		font-size: 6rem;
		font-style: italic;
	}
</style>
