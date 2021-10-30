-- Avelraan
delete from dbo.Acts;
delete from dbo.Episodes;


declare @huurlingId UNIQUEIDENTIFIER = '6F321455-6A2E-418A-9AD4-5432C2CAB32D';
declare @aquilaId UNIQUEIDENTIFIER = 'FD00B041-2661-4027-9F92-69C2A681C70A';


--Episodes
insert into dbo.Episodes (Id, [Name], [Date], [Prologue], [Epilogue], [Story])
values 
	(
		-- Huurling
		@huurlingId,
		'Huurling',
		'0',
		'Duty, honor… payment. They’re all the same to you.',
		'You go about your day.',
		''
	),
	(
		-- Aquila Foedus
		@aquilaId, 
		'Aquila Foedus', 
		'-933', 
		'After the death of the old king, his children fought over the crown of Farlindor. Quintus and Maedra, having the backing of the northern duchies, took over the majority of the land army, while Lisandros and Eneer fled to the south of the large island kingdom, while also retaining control over an impressive fleet. The conflict that followed led to all out civil war between the brothers, that took place mostly in the large valleys of Tael’dorthir mountains, neither gaining an upper hand during the early years. The established states in the south, Londorth and Ennuy, controlled the trade routes between the fractured old kingdom with the rest of the continent, depleting the resources of the northern side and isolating Quintus and his armies to only what the windswept plains of Farlindor could produce. Any land assault was always repulsed by the well prepared troops of Haelva, capital city of Farlindor, leading to significant losses on the southern side, as was the case during the Battle of Ruotsgen Valley approximately 900 years before the Aeldanic calendar. The battle was made famous by the charge led by Quintus of Haelva, clad in a striking golden armour, shining in the bright afternoon sun. Although being unhorsed soon after, he made his way back to the camp, took another charger and repeated the attack. His retinue hacked their way to Lisandros which was waiting on a hill further away at the edge of the battle. When Quintus found himself close enough he pulled out his father’s crown and threw it towards the banner bearers of his brother, only to see his horse die under him once again. The act itself was a statement of how things would go for the young prince, while also causing massive disarray in between the allies nobility, since both of the older brothers wished to ascend to the throne of Haelva. Lisandros’ forces withdrew from the battle soon after, and the ensuing military confrontation ended with haelvan victory.',
		'The black ships of Haelva reached the lands of Seracleea.',
		'Avelraan'
	);


-- Acts
insert into dbo.Acts (Id, [Name], [Difficulty], [EpisodeId], [ActNumber], [Description])
values
	(
		-- Huurling act 1
		(select NEWID()),
		'Huurling act 1: sellsword',
		'Random',
		@huurlingId,
		1,
		'The world turns, the days pass...'
	),
	(
		-- Aquila act 1
		(select NEWID()),
		'Aquila act 1: the eagle and the sea',
		'Random',
		@aquilaId,
		1,
		'Aware that the southern kingdoms will reorganize their armies and attack into Farlindor, Quintus expected a seaborne invasion. In an attempt to prevent being outnumbered in his own lands, he ordered the transport of ships from the eastern ocean to the fiery sea of Gaerlith. He boarded his finest warriors and set sail along the western coast to the Kingdom of Ennuy. The hazardous trip through plumes of ash coming from the underwater volcanoes cost him dearly as the ships entered one by one into the darkened lands of Mount Moot. The air was noxious and barely breathable, the winds burned the skin. Some of the ships fell under the water due to the gases rising upwards, disappearing forever under the waves. Others, more fortunate, would see their masts set ablaze, watching with dread how the fire spreads slowly against their efforts. It was an impossible feat to navigate these waters, and the men of Haelva were losing hope. As customary, Quintus would travel with a golden eagle, a beast indigenous to the peaks of Farlindor. In a desperate attempt to find a path towards land, he used a small lantern tied to the eagles’ legs and set it free, ordering every ship to follow it. As the eagle made its way into the black sky, trying to get above the fumes, it quickly found its way westward and the men rowed with renewed hope. Quintus’ ship survived the trip and landed on the tall shores of Ennuy, surprising the old coastal watchtowers. The bay where his troops landed was later called the Ashen Men, due to the fact that his troops covered in ash were in stark contrast with the white sands of Ennuy. Soon after debarking his men, Quintus found the eagle lying dead on the sand, its wings spread to their fullest in a last attempt to fly again. This event, witnessed by many around him, was taken as an omen and soon after became the symbol of his house, the black eagle on a white background. Quintus took water and ordered the eagle to be cleaned, but not removed, it’s golden feathers shining in the afternoon sky for all troops to see as they came ashore.'
	);


