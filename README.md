# 2021
To celebrate Pi Day, I created this nice but useless tool to deploy an ASCII Art to github contribution history.

# Usage
- git clone
- cd src
- dotnet run \<yourFileName> \<someGithubRepository>
- enjoy your ascii art work on github

# Comments
\<yourFileName> should be a text file with the next interpretation:
for each char, a day will be filled with commits in github
- (space) : no commits
- 'x' : 3 commits(light green)
- 'X' : 9 commits(dark green) 

Use maximum 52 chars width and 7 chars height.

\<someGithubRepository> in order to deploy to another git account, use github PAT when providing the url to the repository.
