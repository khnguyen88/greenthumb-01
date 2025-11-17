import { definePreset } from '@primeuix/themes';
import { bootstrapApplication } from '@angular/platform-browser';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeuix/themes/aura';

export const MyPreset = definePreset(Aura, {
  semantic: {
    primary: {
      50: '{purple.50}',
      100: '{purple.100}',
      200: '{purple.200}',
      300: '{purple.300}',
      400: '{purple.400}',
      500: '{purple.500}',
      600: '{purple.600}',
      700: '{purple.700}',
      800: '{purple.800}',
      900: '{purple.900}',
      950: '{purple.950}',
    },
    colorScheme: {
      light: {
        primary: {
          color: '{purple.800}',
          inverseColor: '#ffffff',
          hoverColor: '{purple.600}',
          activeColor: '{purple.800}',
        },
        highlight: {
          background: '{purple.950}',
          focusBackground: '{purple.700}',
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
          color: '{purple.400}',
          inverseColor: '{purple.950}',
          hoverColor: '{purple.100}',
          activeColor: '{purple.200}',
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
  // components: {
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
  //           background: 'purple',
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
  // },
});
