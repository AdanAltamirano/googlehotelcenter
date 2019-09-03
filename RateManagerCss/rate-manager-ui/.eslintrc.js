module.exports = {
    root: true,
    env: {
        node: true,
    },
    globals: {
        $: true,
    },
    extends: [
        'plugin:vue/essential',
        '@vue/airbnb',
    ],
    rules: {
        'no-console': process.env.NODE_ENV === 'production' ? 'error' : 'off',
        'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
        'no-underscore-dangle': [2, { allowAfterThis: true }],
        'no-param-reassign': ['error', { props: false }],
        'max-len': [2, 120, 4, { ignoreUrls: true }],
        indent: ['error', 4],
    },
    parserOptions: {
        parser: 'babel-eslint',
    },
};
