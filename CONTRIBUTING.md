# Contributing Guidelines

## Development Setup

1. Clone the repository
2. Follow setup guides in `docs/setup/`
3. Create a feature branch: `git checkout -b feature/your-feature`
4. Make your changes
5. Run tests: `dotnet test` (backend) and `npm test` (frontend)
6. Commit with descriptive message
7. Push and create Pull Request

## Commit Message Format

```
type(scope): subject

body

footer
```

Types: feat, fix, docs, style, refactor, test, chore

## Code Style

- Backend: Follow C# conventions
- Frontend: ESLint + Prettier (automatic)
