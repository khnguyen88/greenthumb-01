import { definePreset } from '@primeuix/themes';
import { bootstrapApplication } from '@angular/platform-browser';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeuix/themes/aura';

export const MyPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '{green.50}',
      100: '{green.100}',
      200: '{green.200}',
      300: '{green.300}',
      400: '{green.400}',
      500: '{green.500}',
      600: '{green.600}',
      700: '{green.700}',
      800: '{green.800}',
      900: '{green.900}',
      950: '{green.950}',
    },
    colorScheme: {
      light: {
        primary: {
          color: '{green.700}',
          inverseColor: '#ffffff',
          hoverColor: '{purple.500}',
          activeColor: '{purple.700}',
        },
        highlight: {
          background: '{green.950}',
          focusBackground: '{green.700}',
          color: '#ffffff',
          focusColor: '#ffffff',
        },
        surface: {
          0: '#ffffff',
          50: '{zinc.50}',
          100: '{zinc.100}',
          200: '{zinc.200}',
          300: '{zinc.300}',
          400: '{zinc.400}',
          500: '{zinc.500}',
          600: '{zinc.600}',
          700: '{zinc.700}',
          800: '{zinc.800}',
          900: '{zinc.900}',
          950: '{zinc.950}',
        },
      },
      dark: {
        primary: {
          color: '{green.500}',
          inverseColor: '{green.950}',
          hoverColor: '{purple.200}',
          activeColor: '{purple.300}',
        },
        highlight: {
          background: 'rgba(250, 250, 250, .16)',
          focusBackground: 'rgba(250, 250, 250, .24)',
          color: 'rgba(255,255,255,.87)',
          focusColor: 'rgba(255,255,255,.87)',
        },
        surface: {
          0: '#ffffff',
          50: '{slate.50}',
          100: '{slate.100}',
          200: '{slate.200}',
          300: '{slate.300}',
          400: '{slate.400}',
          500: '{slate.500}',
          600: '{slate.600}',
          700: '{slate.700}',
          800: '{slate.800}',
          900: '{slate.900}',
          950: '{slate.950}',
        },
      },
    },
  },
  components: {
    textarea: {
      colorScheme: {
        light: {
          root: {
            background: '{surface.50}',
          },
        },
        dark: {
          root: {
            background: '{surface.900}',
          },
        },
      },
    },
    //   menubar: {
    //     colorScheme: {
    //       light: {
    //         root: {
    //           background: 'blue',
    //         },
    //         item: {
    //           focusBackground: 'red',
    //         },
    //       },
    //       dark: {
    //         root: {
    //           background: 'green',
    //         },
    //         item: {
    //           focusBackground: 'green',
    //         },
    //       },
    //     },
    //   },
    //   button: {
    //     extend: {
    //       accent: {
    //         color: 'yellow',
    //         inverseColor: '#ffffff',
    //       },
    //     },
    //     css: ({ dt }) => `
    //   .p-button {
    //     background: ${dt('button.accent.color')};
    //     color: ${dt('button.accent.inverse.color')};
    //     transition-duration: ${dt('my.transition.fast')};
    //   }`,
    //   },
  },
});
