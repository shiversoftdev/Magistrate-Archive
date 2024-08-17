# Magistrate Practice Images Archive
This repository is an archive of the work done by the Magistrate/Dynamix team for practice images for the CyberPatriot competition. This includes source to most of our tricks, the engine, and some of our design documents.
## About the Engine
Magistrate's goal was to operate as a standalone, perfectly secure scoring solution that was resistant to API monitoring and other tricks that provided students with an unfair advantage during a competition.
- Firstly, all checks in the engine are generated from a config file at compile time, which then is placed as code only inside the main engine loop.
- Checks are designed with the philosophy that players must recover completely destroyed data to complete a check. For example: a registry check doesnt know the key name or value that it wants to compare against, only that one subkey of a general path combined with its value will produce a specific hash value.
- Every check can create a hash value which serves as an AES decryption key (paired with a random IV generated at compile time), which protects both the check description and a special identifier that allows us to validate that someone has indeed decrypted the solution text.
- Most design philosophy is based around a "foggy answer" ideology; if the engine doesnt know what it wants exactly, neither do the players.
- This would only work for exact comparisons, however, by using modulus math and other simple tricks, ranges and other types of checks can be utilized (substrings can produce rolling hashes, etc.)
- I am 100% confident that with the correct creativity, truly unbreakable images can be created with these techniques.
## About the Images
- We designed most images with a *real attack path*. This means that we actually simulated a real world attack on the image, leaving all forensic traces in tact, and then ask students to figure out what happened.
- We strongly believe that this is a necessary evolution to the competition, and that students should be learning about what real attack techniques look like.
- We also apply game design principles to try to create an engaging, polished experience for the competitors.
## Final Thoughts
We tried our best to lead by example. With the correct creativity and motivation, images can be created with the proper depth and engagement required to keep competitors on their toes year to year. We appreciate the CyberPatriot organizers, competitors, and community, and we hope that this information benefits you all.\
\
Thanks for playing!