# CVVC/VCCV Helper

Very much incomplete.

A plugin that should help with inputting lyrics into UTAU. The idea is that you would put in each syllable, and this plugin would break the syllables apart into their component sounds. For example, the English VCCV word "flossing":

    input: [fla][sing]
    output: [-fl][_la][a s][s1][1ng-]

My plan is to support:

- English VCCV (CZloid, etc.)
- French CVVC and French CVVC+ (FRAloids)
- Chinese CVVC (Xia Yuyao/夏语遥, etc.)

simply because these are the three types of voicebanks (alongside Japanese VCV) that I use. But this is a very early plan that might change in the future (if all goes well, I would also be able to read additional presets, like Delta's AutoCVVC).

Speaking of AutoCVVC, this basically does the same thing as that, but for some reason, that one doesn't work on my computer. Additionally, I plan for this to be optimized for consonant clusters that exist in English and French (CC sounds, like `[bl]`).

Makes heavy use of riipah's [utauBulkEnvelopeEditor](https://github.com/riipah/utauBulkEnvelopeEditor) code, especially since there don't seem to be any guides in English for making UTAU plugins.