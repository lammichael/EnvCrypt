require 'albacore'
require 'nokogiri'

################################
#  Variables
################################

#  Common directory locations
CheckoutDir=File.expand_path('.', File.join(File.dirname(__FILE__), '..'))
SourceDir=File.join(CheckoutDir, 'src')
OutputDir=File.join(CheckoutDir, 'out')

CommonAssemblyInfoFilePath=File.join SourceDir, 'CommonAssemblyInfo.cs'

#  What we're building
SLNFilePath=File.join SourceDir, 'EnvCrypt.Core.sln'

#  Nuget binary
NuGetExe = File.join CheckoutDir, 'tools', 'NuGet.2.8.5', 'NuGet.exe'
NuSpecFilePath = File.join(SourceDir, 'EnvCrypt.Core.nuspec')

################################
#  Tasks
################################

desc 'restore all nugets as per the packages.config files'
task(:packagerestore) do
  sh(NuGetExe, 'restore', SLNFilePath)
end


task :createoutputdir do
  Dir.mkdir OutputDir unless File.directory?(OutputDir)
end


task :cleanoutput do
  rm_rf OutputDir
end


desc 'MSBuild clean'
build :msbuildclean do |b|
  b.sln  = SLNFilePath
  b.target = 'Clean'
  b.prop 'Configuration', 'Release'	# call with 'key, value', to specify a MsBuild property
  b.logging = 'normal'					# detailed logging mode. The available verbosity levels are: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]
  # b.be_quiet						# opposite of the above
  b.nologo							# no Microsoft/XBuild header output
  #b.prop 'NoWarn', '1591'			# Don't warn about missing XML doc
end


desc 'MSBuild compile'
build :msbuildcompile do |b|
  b.sln  = SLNFilePath
  b.target = 'Build'				# call with an array of targets or just a single target
  b.prop 'Configuration', 'Release'	# call with 'key, value', to specify a MsBuild property
  #b.clp 'ShowEventId'				# any parameters you want to pass to the console logger of MsBuild
  b.logging = 'm'					# detailed logging mode. The available verbosity levels are: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]
  # b.be_quiet						# opposite of the above
  b.prop 'NoWarn', '1591'			# Don't warn about missing XML doc
end


################################
#  NuSpec
################################
task(:versionnuspec) do
  fileContents = File.read(NuSpecFilePath)
  assemblyVersion = get_assembly_version()
  puts assemblyVersion
  
  #puts "sdfsdf<version>blahM</version>der".gsub(/(<version>)(.*)(<\/version>)/, '\11.2332\3')
  #puts "Z_sdsd: sdsd".gsub(/^(Z_.*): .*/, '\1')
  
  replacementText = "\\1" + assemblyVersion + "\\3"
  newFileContents = fileContents.gsub(/(<version>)(.*)(<\/version>)/, replacementText)
  File.open(NuSpecFilePath, 'w') {|f| f.write(newFileContents) }
end

desc "Get the Assembly's version number"
def get_assembly_version()
   fileContents = File.read(CommonAssemblyInfoFilePath)

   assemblyVersion = fileContents.match(/^\[assembly: AssemblyVersion\("(.*)"\)/i).captures[0]
   return assemblyVersion
end


desc "Create the nuget package"
task(:nugetpackage) do
  sh(NuGetExe, 'pack', NuSpecFilePath, '-symbols', '-OutputDirectory', OutputDir)
end


################################
#  Targets
################################
task :compile => :msbuildcompile
task :clean => [:msbuildclean, :cleanoutput]
task :package => [:createoutputdir, :compile, :nugetpackage]
task :nugetpackage => :versionnuspec

task :ci => [:clean, :packagerestore, :compile]
task :default => :ci